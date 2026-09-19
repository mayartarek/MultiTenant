using MultiTenant.Entities;

namespace MultiTenant.Service
{
    public interface IProductService
    {
        Task<Product> GetProductAsync(int id);
        Task<Product> CreateProductAsync(Product product);
        Task<List<Product>> GetAllProductsAsync();  

    }
}
