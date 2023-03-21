using Microsoft.AspNetCore.Http;
using OnlineEducation.Shared;
using System.Reflection;

namespace OnlineEducation.Ultils
{
    public class FileHandlers
    {
        public async static Task<string> UploadFile(IFormFile file, Guid creatorId)
        {
            if(file == null) {
                var fullPath = Path.Combine(AppConst.LocalFileSavePath, creatorId.ToString());
                using (var stream = System.IO.File.Create(fullPath))
                {
                    await file.CopyToAsync(stream);
                    return fullPath;
                }
            }

            return string.Empty;
        }
    }
}
