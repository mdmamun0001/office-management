using Microsoft.AspNetCore.Hosting;

namespace Zdm_management.CustomHelper
{
    public static class Helper
    {
        public static string? UploadFile(IFormFile image, string Folder, IWebHostEnvironment WebHostEnvironment)
        {
            string? fileName = null;
            if (image != null && WebHostEnvironment != null && Folder != null)
            {
                Guid id = Guid.NewGuid();
                if (!Directory.Exists(Path.Combine(WebHostEnvironment.WebRootPath, Folder)))
                    Directory.CreateDirectory(Path.Combine(WebHostEnvironment.WebRootPath, Folder));
                string uploadDir = Path.Combine(WebHostEnvironment.WebRootPath, Folder);
                fileName = id + "_" + image.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }

            return fileName;
        }
        public static void  DeleteFile( IWebHostEnvironment  WebHostEnvironment, string FileName, string  Folder)
        {
            if(WebHostEnvironment != null && FileName != null && Folder != null) 
            {
                string FileLocation = Path.Combine(WebHostEnvironment.WebRootPath, Folder);

                string ExistingFilePath = Path.Combine(FileLocation, FileName.Trim());
                if (System.IO.File.Exists(ExistingFilePath))
                {
                    System.IO.File.Delete(ExistingFilePath);
                }
            }
            
        }
    }
}
