using Entities;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace Services
{
    public class ProductsService : IProductsService
    {
        //Private Fields
        private readonly IProductsRepository _productsRepository;
        public ProductsService(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }
        public Task<ProductResponse> AddProduct(ProductAddRequest? productRequest)
        {
            //Validation: ProductRequest can't be null
            if (productRequest == null)
            {
                throw new ArgumentNullException(nameof(productRequest), "Product add request cannot be null");
            }

            //Validation: ProductName can't be null
            if (productRequest.ProductName == null)
            {
                throw new ArgumentException(nameof(productRequest.ProductName));
            }

            //Validation: ProductName should not exceed 50 characters
            if(productRequest.ProductName.Length >50)
            {
                throw new ArgumentOutOfRangeException(nameof(productRequest.ProductName), "Product name should not exceed 50 characters");
            }

            //Validation: Duplicate ProductName
            if (_productsRepository.GetProductByName(productRequest.ProductName).Result != null)
            {
                throw new ArgumentException(productRequest.ProductName + " is already exists");
            }

            //Convert object from CategoryRequest to Category
            Product product = productRequest.ToProduct();

            //Generate CategoryID
            product.ProductID = Guid.NewGuid();

            //Add product object into products
            _productsRepository.AddProduct(product);
            return Task.FromResult(product.ToProductResponse());
        }

        public  Task<List<ProductResponse>> GetAllProducts()
        {
            var result = _productsRepository.GetAllProducts().Result.Select(s => s.ToProductResponse()).ToList();
            return Task.FromResult(result);
        }

        public Task<ProductResponse> GetProductByID(Guid? productId)
        {
            if(productId==null)
            {
                throw new ArgumentNullException(nameof(productId), "Product ID cannot be null");
            }
            var matchingProduct = _productsRepository.GetProductByID(productId);

            if(matchingProduct.Result==null)
            {                
                throw new ArgumentException($"Product with ID {productId} does not exists");
            }       

            return Task.FromResult(matchingProduct.Result.ToProductResponse());
        }
        public Task<List<ProductResponse>> GetProductBySearch(string? search)
        {
            if (string.IsNullOrEmpty(search))
            {
                var result = _productsRepository.GetAllProducts().Result.Select(s => s.ToProductResponse()).ToList();
                return Task.FromResult(result);
            }
            var matchingProduct = _productsRepository.GetProductBySearch(search);

            if (matchingProduct.Result == null)
            {
                throw new ArgumentException("No product found with the given search string");
            }

            return Task.FromResult(matchingProduct.Result.Select(s=>s.ToProductResponse()).ToList()); 
        }

        public Task<ProductResponse> UpdateProduct(ProductUpdateRequest? productRequest)
        {
            
           if (productRequest == null)
            {
                throw new ArgumentNullException(nameof(productRequest), "Product update request cannot be null");
            }

            //Validation: ProductName should not exceed 50 characters
            if (productRequest.ProductName.Length > 50)
            {
                throw new ArgumentOutOfRangeException(nameof(productRequest.ProductName), "Product name should not exceed 50 characters");
            }

            //Get matching category object to update
            Product? matchingProduct = _productsRepository.GetProductByID(productRequest.ProductID).Result;

            if(matchingProduct != null && matchingProduct.ProductName != productRequest.ProductName)
            {
                //Validation: Duplicate ProductName
                if (_productsRepository.GetProductByName(productRequest.ProductName).Result != null)
                {
                    throw new ArgumentException(productRequest.ProductName + " is already exists");
                }
            }

            if (matchingProduct == null)
            {
                throw new ArgumentException("Given Product ID does not exists");
            }

            //Update details
            matchingProduct.ProductName = productRequest.ProductName;
            _productsRepository.UpdateProduct(matchingProduct);

            return Task.FromResult(matchingProduct.ToProductResponse());

        }
        public Task<bool> DeleteProduct(Guid productId)
        {
            var deleteResult = _productsRepository.DeleteProduct(productId);
            return deleteResult;
        }
    }
}
