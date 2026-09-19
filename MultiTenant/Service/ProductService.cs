using Microsoft.EntityFrameworkCore;
using MultiTenant.Entities;

namespace MultiTenant.Service
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDBContext _dBContext;

        public ProductService(ApplicationDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            await _dBContext.Products.AddAsync(product);
            await _dBContext.SaveChangesAsync();
            return product;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
           return await _dBContext.Products.ToListAsync();

        }

        public async Task<Product> GetProductAsync(int id)
        {
            return await _dBContext.Products.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
