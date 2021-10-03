using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Utility.Helpers
{
    public abstract class BaseApiClient
    {
        protected readonly IHttpClientFactory _clientFactory;
        protected readonly HttpClient _httpClient;

        protected abstract HttpClient SetHttpClient();

        public BaseApiClient(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _httpClient = SetHttpClient();
        }

        public async Task<TResponse> GetApiAsync<TResponse>(string requestUri)
            where TResponse : class
        {
            var response = await _httpClient.GetAsync(requestUri);
            return await ReadResponse<TResponse>(response);
        }

        public async Task<TResponse> PostApiAsync<TResponse, TRequest>(string requestUri, TRequest request)
            where TResponse : class
            where TRequest : class
        {
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(requestUri, httpContent);
            return await ReadResponse<TResponse>(response);
        }

        public async Task<TResponse> PostMultipartApiAsync<TResponse, TRequest>(string requestUri, TRequest request, IFormFile formFile = null, string name = null)
            where TResponse : class
            where TRequest : class
        {
            MultipartFormDataContent multiContent = new MultipartFormDataContent();

            Type type = request.GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (var property in properties)
            {
                var jsonPropertyName = property.GetCustomAttributes<JsonPropertyAttribute>().Select(x => x.PropertyName).FirstOrDefault();
                multiContent.Add(new StringContent(Convert.ToString(property.GetValue(request))), jsonPropertyName);
            }

            if (formFile != null)
            {
                multiContent.Add(new StreamContent(formFile.OpenReadStream())
                {
                    Headers =
                {
                    ContentLength = formFile.Length,
                    ContentType = new MediaTypeHeaderValue(formFile.ContentType)
                }
                }, name, formFile.FileName);
            }
            var response = await _httpClient.PostAsync(requestUri, multiContent);
            return await ReadResponse<TResponse>(response);
        }

        public async Task<TResponse> PutApiAsync<TResponse, TRequest>(string requestUri, TRequest request)
            where TResponse : class
            where TRequest : class
        {
            HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(requestUri, httpContent);
            return await ReadResponse<TResponse>(response);
        }

        private async Task<TResponse> ReadResponse<TResponse>(HttpResponseMessage response)
            where TResponse : class
        {
            if (response != null)
            {
                //try
                //{
                var responseStream = await response.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<TResponse>(responseStream);

                return responseData;
                //}
                //catch (Exception exc)
                //{
                //    return null;
                //}
            }
            else
            {
                return null;
            }
        }
    }
}
