using Microsoft.AspNetCore.Http;
using System.IO;
using System;

namespace Tourist_Place.Models.Others
{
    public class UploadFileControl
    {
        public static string FileName(IFormFile file, string PathToUpload)
        {
            string uniqueFileName = null;
            if (file != null)
            {
                string uploadsFolder = PathToUpload;
                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
