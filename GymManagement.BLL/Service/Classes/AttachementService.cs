using GymManagement.BLL.Service.InterFaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.BLL.Service.Classes
{
    public class AttachementService : IAttachementServices
    {
        public AttachementService(ILogger<AttachementService> logger ,IWebHostEnvironment env)
        {
            this.logger = logger;
            this.env = env;
        }
        private readonly long maxFileSize = 5 * 1024 * 1024;
        private readonly string[] allowedExtentions = { ".png",".jpeg",".jpg" };
        private readonly ILogger logger;
        private readonly IWebHostEnvironment env;

        public bool Delete(string filename, string FolderName)
        {
            if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(FolderName)) return false;

            try
            {
                var fullPath = Path.Combine(env.ContentRootPath,FolderName, filename);
                if(!File.Exists(fullPath)) return false;

                File.Delete(fullPath);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed To Delete Photo");
                return false;
            }

        }

        public (Stream stream, string contentType)? GetFile(string filename, string FolderName)
        {
            if (string.IsNullOrWhiteSpace(filename) || string.IsNullOrEmpty(FolderName)) return null;
            var fullPath = Path.Combine(env.ContentRootPath, FolderName, filename);
            if (!File.Exists(fullPath)) return null;

            var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
            var extention = Path.GetExtension(filename).ToLowerInvariant();
            var contentType = extention switch
            {
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                _=> "application/octet-stream"
            };

            return (stream, contentType);
        }

        public async Task<string>? UploadAsync(Stream filestream, string filename, string Foldername, CancellationToken ct =default)
        {
            if (filestream == null || !filestream.CanRead || filestream.Length == 0) return null;
            if(filestream.Length > maxFileSize)
            {
                logger.LogWarning("Rejected File Too Large");
                return null;
            }
            var extention = Path.GetExtension(filename);

            if(string.IsNullOrWhiteSpace(extention) || !allowedExtentions.Contains(extention)) 
            {
                logger.LogWarning("Rejected Wrong Extention");
                return null;
            }

            var UploadedFolder = Path.Combine(env.ContentRootPath, Foldername);
            Directory.CreateDirectory(UploadedFolder);

            var storedFileName = $"{Guid.NewGuid()}{extention}";

            var filePath = Path.Combine(UploadedFolder, storedFileName);

            try
            {
                await using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
                await filestream.CopyToAsync(fs);
                return storedFileName;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Filed To Upload Photo");
                return null;
            }
        }
    }
}
