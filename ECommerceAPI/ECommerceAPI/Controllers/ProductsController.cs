
using ECommerceAPI.Data;
using ECommerceAPI.Data.Entities;
using ECommerceAPI.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ECommerceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IImageService _imageService;

        public ProductsController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment, IImageService imageService)
        {
            _webHostEnvironment = webHostEnvironment;
            _dbContext = dbContext;
            _imageService = imageService;

        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductFormModel productFormModel)
        {
            if (productFormModel.ImageFile == null || productFormModel.ImageFile.Length == 0)
            {
                return BadRequest("Image file is required.");
            }

            var uniqueFileName = await _imageService.SaveImage(productFormModel.ImageFile);

            var product = new Product
            {
                Code = productFormModel.Code, 
                Name = productFormModel.Name,
                Category = productFormModel.Category,
                MinimumQuantity = productFormModel.MinimumQuantity,
                Price = productFormModel.Price,
                DiscountRate = productFormModel.DiscountRate,
                ImagePath = uniqueFileName
            };

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { code = product.Code }, product);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _dbContext.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
    }
    }
