using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IO;
using VibeQuest.Dto;
using VibeQuest.Service.Contracts;
using VibeQuest.Utility.Extensions;
using VibeQuest.Utility.Helpers;
using VibeQuest.Utility.JWT;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace VibeQuest.Api.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task Invoke(HttpContext context,
            IAuditLogService auditLogService,
            ICurrentUser currentUser)
        {
            AuditLogsDto auditLogsDto = new AuditLogsDto(Guid.NewGuid());
            auditLogsDto = await LogRequest(context, auditLogsDto);
            var stopWatch = Stopwatch.StartNew();
            try
            {
                await _next(context);
                stopWatch.Stop();
            }
            catch (Exception exc)
            {
                stopWatch.Stop();
                var response = context.Response;
                response.ContentType = "application/json";

                switch (exc)
                {
                    case UserFriendlyException:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
                auditLogsDto.Exception = exc?.ToString();
                var result = JsonSerializer.Serialize(new { message = exc?.Message });
                await response.WriteAsync(result);
            }
            finally
            {
                auditLogsDto.UserId = !currentUser.Id.IsNullOrWhiteSpace() ? new Guid(currentUser.Id) : null;
                auditLogsDto.ExecutionDuration = stopWatch.ElapsedMilliseconds;
                auditLogsDto.HttpStatusCode = context.Response?.StatusCode ?? 0;
                auditLogsDto.Comments = $"Controller: {context.Request.RouteValues["controller"]?.ToString()}, Action: {context.Request.RouteValues["action"]?.ToString()}";
                await auditLogService.InsertAuditLog(auditLogsDto);
            }
        }

        private async Task<AuditLogsDto> LogRequest(HttpContext context, AuditLogsDto auditLogsDto)
        {
            context.Request.EnableBuffering();

            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);

            auditLogsDto.BrowserInfo = context.Request?.Headers["User-Agent"].ToString();
            auditLogsDto.ClientIpAddress = context.Request?.HttpContext.Connection.RemoteIpAddress?.ToString();
            auditLogsDto.HttpMethod = context.Request?.Method;
            auditLogsDto.Parameters = ReadStreamInChunks(requestStream);
            auditLogsDto.Url = $"{context.Request.Scheme}://{context.Request?.Host}{context.Request?.Path}{(context.Request.QueryString.HasValue ? context.Request?.QueryString.Value : "")}";
            auditLogsDto.ExecutionTime = DateTime.UtcNow;

            context.Request.Body.Position = 0;
            return auditLogsDto;
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;

            stream.Seek(0, SeekOrigin.Begin);

            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);

            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;

            do
            {
                readChunkLength = reader.ReadBlock(readChunk,
                                                   0,
                                                   readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);

            return textWriter.ToString();
        }
    }
}
