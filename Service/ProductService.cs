using OnlineRetailStoreV01.Models;
using OnlineRetailStoreV01.Repository;

namespace OnlineRetailStoreV01.Service
{
    public class ProductService:IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository) 
        {
            _productRepository=productRepository;
        }

        public async Task AddProductAsync(Product Product)
        {
            await _productRepository.AddProductAsync(Product);
        }

        public async Task DeleteProductAsync(int ProductId)
        {
            await _productRepository.DeleteProductAsync(ProductId);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllProductsAsync();
        }

        public async Task<Product> GetProductByIdAsync(int ProductId)
        {
            return await _productRepository.GetProductByIdAsync(ProductId);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(ProductCategory category)
        {
            return await _productRepository.GetProductsByCategoryAsync(category);
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            return await _productRepository.SearchProductsAsync(searchTerm);
        }

        public async Task UpdateProductAsync(int ProductId, Product updatedProduct)
        {
             await _productRepository.UpdateProductAsync(ProductId, updatedProduct);
        }
    }
}
