using Entities;

namespace RepositoryContracts
{
    /// <summary>
    /// Represents data access logic for managing categories entity
    /// </summary>
    public interface IProductsRepository
    {
        /// <summary>
        /// Adds a new product to the data store
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Returns the product object after adding it to the table</returns>
        Task<Product> AddProduct(Product product);

        /// <summary>
        /// Updates an existing product in the data store
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Returns the updated product</returns>
        Task<Product> UpdateProduct(Product product);
        /// <summary>
        /// Returns all products from the data store
        /// </summary>
        /// <returns>All products from the table</returns>
        Task<List<Product>> GetAllProducts();
        /// <summary>
        /// Returns a product object based on the given category name
        /// </summary>
        /// <param name="productName"></param>
        /// <returns>Returns the matching category object</returns>
        Task<Product> GetProductByName(string productName);
        /// <summary>
        ///  Returns a product object based on the given categoryId
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Returns the matching product object</returns>
        Task<Product> GetProductByID(Guid? productId);
        /// <summary>
        /// Returns a product object based on the given search string 
        /// </summary>
        /// <param name="search"></param>
        /// <returns>Returns all matches product based on the given search string</returns>
        Task<List<Product>> GetProductBySearch(string search);
        /// <summary>
        /// Deletes the product with the specified product Id.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to delete.</param>
        /// <returns>True, if the category was successfully deleted; otherwise, false.</returns>
        Task<bool> DeleteProduct(Guid productId);
    }
}
