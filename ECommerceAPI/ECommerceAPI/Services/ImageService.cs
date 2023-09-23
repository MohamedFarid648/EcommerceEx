
using ECommerceAPI.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ECommerceAPI.Services
{
    public class ImageService : IImageService
    {
        private readonly string _imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Images");

        public async Task<string> SaveImage(IFormFile imageFile)
        {
            if (!Directory.Exists(_imageFolderPath))
            {
                Directory.CreateDirectory(_imageFolderPath);
            }

            if (imageFile != null)
            {

                var uniqueFileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
                var filePath = Path.Combine(_imageFolderPath, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                return uniqueFileName;
            }
            else
            {
                return null;
            }
        }
    }
}
