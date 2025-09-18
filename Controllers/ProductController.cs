using AutoMapper;
using Ecomm_demo.Entities;
using Ecomm_demo.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecomm_demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] int? categoryId = null)
        {
            IEnumerable<Product> products;

            if (categoryId.HasValue)
            {
                products = await _productRepository.GetProductsByCategoryAsync(categoryId.Value);
            }
            else
            {
                products = await _productRepository.GetActiveProductsAsync();
            }

            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(productDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _productRepository.GetProductWithDetailsAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var productDto = _mapper.Map<ProductDto>(product);
            return Ok(productDto);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto createProductDto)
        {
            // Validate category exists
            if (!await _categoryRepository.ExistsAsync(createProductDto.CategoryId))
            {
                return BadRequest("Category not found");
            }

            var product = _mapper.Map<Product>(createProductDto);
            var createdProduct = await _productRepository.AddAsync(product);

            // Get with details for proper mapping
            var productWithDetails = await _productRepository.GetProductWithDetailsAsync(createdProduct.ProductId);
            var productDto = _mapper.Map<ProductDto>(productWithDetails);

            return CreatedAtAction(nameof(GetProduct), new { id = productDto.ProductId }, productDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, CreateProductDto updateProductDto)
        {
            if (!await _productRepository.ExistsAsync(id))
            {
                return NotFound();
            }

            if (!await _categoryRepository.ExistsAsync(updateProductDto.CategoryId))
            {
                return BadRequest("Category not found");
            }

            var product = _mapper.Map<Product>(updateProductDto);
            product.ProductId = id;
            await _productRepository.UpdateAsync(product);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleted = await _productRepository.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

