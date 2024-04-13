using OnlineRetailStoreV01.Models;

namespace OnlineRetailStoreV01.Repository
{
    public interface IProductRepository
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
