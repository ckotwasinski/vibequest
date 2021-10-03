using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VibeQuest.Utility.Helpers
{
    public class FileHelper
    {
        public static async Task<string> CreateFileAsync(IFormFile file, string webRootPath,string imagePath)
        {
            string fileExtension = FileExtention(file.FileName);
            string fileName = FileName(file.FileName, fileExtension);
            string physicalPath = string.Format(webRootPath + "/{0}", Constants.GetUrl(imagePath, fileName));

            using (var fileStream = new FileStream(physicalPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return fileName;
        }
        public static string FileExtention(string filename)
        {
            if(!string.IsNullOrWhiteSpace(filename))
                return filename.Substring(filename.LastIndexOf("."));
            return "";
        }
        public static string FileName(string filename,string fileExtension)
        {
            if (!string.IsNullOrWhiteSpace(filename))
                return filename.Replace(fileExtension, "").Replace(" ", "-").Replace("'", "").Replace("(", "-").Replace(")", "-") + "-" + DateTime.UtcNow.Ticks + fileExtension;
            return "";
        }
    }
}
