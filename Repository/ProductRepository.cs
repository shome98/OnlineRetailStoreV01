using Microsoft.EntityFrameworkCore;
using OnlineRetailStoreV01.Data;
using OnlineRetailStoreV01.Models;

namespace OnlineRetailStoreV01.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;

        public ProductRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddProductAsync(Product Product)
        {
            try
            {
                await _db.Products.AddAsync(Product);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while adding a new Product!!!\n Please try again later. ", ex);
            }

        }

        public async Task DeleteProductAsync(int ProductId)
        {
            try
            {
                var Product = await _db.Products.FindAsync(ProductId);
                if (Product != null)
                {
                    _db.Products.Remove(Product);
                    await _db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while deleting the Product!!!\n Please try again later. ", ex);
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _db.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int ProductId)
        {
            return await _db.Products.FindAsync(ProductId);
        }

        public async Task UpdateProductAsync(int ProductId, Product updatedProduct)
        {
            try
            {
                var existingProduct = await _db.Products.FindAsync(ProductId);
                if (existingProduct != null)
                {
                    existingProduct.ProductName = updatedProduct.ProductName;
                    existingProduct.ProductDescription = updatedProduct.ProductDescription;
                    existingProduct.Price = updatedProduct.Price;
                    existingProduct.ImageUrl = updatedProduct.ImageUrl;
                    existingProduct.Inventory = updatedProduct.Inventory;
                    existingProduct.ProductCategory = updatedProduct.ProductCategory;
                    await _db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while updating the Product!!!\n Please try again later. ", ex);
            }
        }

        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            try
            {
                string formattedSearchTerm = searchTerm.Trim().ToLower();
                return await _db.Products
                    .Where(p =>
                        p.ProductName.ToLower().Replace(" ", "").Contains(formattedSearchTerm.Replace(" ", "")) ||
                        p.ProductDescription.ToLower().Replace(" ", "").Contains(formattedSearchTerm.Replace(" ", "")) ||
                        CalculateLevenshteinDistance(p.ProductName.ToLower(), formattedSearchTerm) <= 2 ||
                        CalculateLevenshteinDistance(p.ProductDescription.ToLower(), formattedSearchTerm) <= 2)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while searching for products!!!\n Please try again later. ", ex);
            }
        }
        private static int CalculateLevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0)
                return m;

            if (m == 0)
                return n;

            for (int i = 0; i <= n; d[i, 0] = i++) ;
            for (int j = 0; j <= m; d[0, j] = j++) ;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[n, m];
        }
        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(ProductCategory category)
        {
            try
            {
                return await _db.Products.Where(p => p.ProductCategory == category).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting the products under the category!!!\nPlease try again later. ", ex);
            }
        }
    }
}
