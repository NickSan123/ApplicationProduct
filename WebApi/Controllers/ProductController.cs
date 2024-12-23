using ApplicationProduct.Application.Interfaces;
using ApplicationProduct.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationProduct.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductController(IAuthService authService, IProductApiService productApiService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly IProductApiService _productApiService = productApiService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            var dadosToken = Request.Headers["Authorization"];
            if (dadosToken.Count < 1)
            {
                return Unauthorized("Camada 8! Voce deve inserir o token");
            }
            var payload = await _authService.ValidateToken(dadosToken);
            if (payload == null)
            {
                return Unauthorized("Tokem invalido ou espirado");
            }

            var products = await _productApiService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var dadosToken = Request.Headers["Authorization"];
            if (dadosToken.Count < 1)
            {
                return Unauthorized("Camada 8! Voce deve inserir o token");
            }
            var payload = await _authService.ValidateToken(dadosToken);
            if (payload == null)
            {
                return Unauthorized("Tokem invalido ou espirado");
            }
            var product = await _productApiService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
    }
}
