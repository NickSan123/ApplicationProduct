using ApplicationProduct.Application.Interfaces;
using ApplicationProduct.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ApplicationProduct.Application.Services
{
    public class ProductApiService(HttpClient httpClient) : IProductApiService
    {
        private readonly HttpClient _httpClient = httpClient;

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var response = await _httpClient.GetAsync("products.json");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<IEnumerable<Product>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"products/{id}.json");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Product>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
