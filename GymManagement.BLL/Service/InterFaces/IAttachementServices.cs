using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.BLL.Service.InterFaces
{
    public interface IAttachementServices
    {
        Task<string>? UploadAsync(Stream filestream, string filename, string Foldername, CancellationToken ct =default);

        bool Delete(string filename,string FolderName);

        (Stream stream, string contentType)? GetFile(string filename ,string FolderName);
    }
}
