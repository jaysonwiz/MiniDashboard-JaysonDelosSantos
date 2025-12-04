using Entities;
using RepositoryContracts;

namespace Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        List<Product> products = new List<Product>();   
        public ProductsRepository()
        {
            // Seed with some sample products
            products.Add(new Product { ProductID = Guid.Parse("11111111-1111-1111-1111-111111111111"), ProductName = "Lenovo Laptop IdeaPAD" });
            products.Add(new Product { ProductID = Guid.Parse("22222222-2222-2222-2222-222222222222"), ProductName = "iPhone 17 Pro Max" });
            products.Add(new Product { ProductID = Guid.Parse("33333333-3333-3333-3333-333333333333"), ProductName = "Sterling Notebook" });
            products.Add(new Product { ProductID = Guid.Parse("44444444-4444-4444-4444-444444444444"), ProductName = "Red Seal Vitamin C" });
            products.Add(new Product { ProductID = Guid.Parse("55555555-5555-5555-5555-555555555555"), ProductName = "Puffer Jacket Medium" });

        }
        public  Task<Product> AddProduct(Product product)
        {
            product.ProductID = Guid.NewGuid();
            products.Add(product);

            return Task.FromResult(product);
        }

        public Task<List<Product>> GetAllProducts()
        {
            return Task.FromResult(products);
        }

        public Task<Product?> GetProductByID(Guid? productId)
        {
            return Task.FromResult(products.FirstOrDefault(w => w.ProductID == productId));
        }

        public Task<Product?> GetProductByName(string productName)
        {
            return Task.FromResult(products.FirstOrDefault(w => w.ProductName == productName));
        }

        public Task<List<Product>> GetProductBySearch(string search)
        {
            return Task.FromResult(products.Where(w => w.ProductName.Contains(search,StringComparison.OrdinalIgnoreCase)).ToList());
        }

        public Task<Product> UpdateProduct(Product product)
        {
            Product? matchingProduct = products.FirstOrDefault(cat => cat.ProductID == product.ProductID);
            matchingProduct!.ProductName = product.ProductName;
            return Task.FromResult(matchingProduct);  
        }
        public Task<bool> DeleteProduct(Guid categoryId)
        {
            Product? matchingProduct = products.FirstOrDefault(cat => cat.ProductID == categoryId);
            if(matchingProduct != null)
            {
                products.Remove(matchingProduct);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }   
    }
}
