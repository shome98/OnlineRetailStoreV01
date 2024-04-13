using OnlineRetailStoreV01.Models;

namespace OnlineRetailStoreV01.Service
{
    public interface IProductService
    {
        Task<Product> GetProductByIdAsync(int ProductId);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task AddProductAsync(Product Product);
        Task DeleteProductAsync(int ProductId);
        Task UpdateProductAsync(int ProductId, Product updatedProduct);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(ProductCategory category);
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
    }
}
