using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ECommerceAPI.Services
{
    
    public interface IImageService
    {
        Task<string> SaveImage(IFormFile imageFile);
    }
}
