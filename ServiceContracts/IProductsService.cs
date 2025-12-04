using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface IProductsService
    {
        /// <summary>
        /// Adds a category object to the list of categories
        /// </summary>
        /// <param name="categoryRequest"></param>
        /// <returns>Returns the category object after adding it (including the newly generated category ID) </returns>
        Task<ProductResponse> AddProduct(ProductAddRequest? categoryRequest);

        /// <summary>
        /// Updates an existing category object in the list of categories
        /// </summary>
        /// <param name="categoryRequest"></param>
        /// <returns>Returns the updated category object after editing the record</returns>
        Task<ProductResponse> UpdateProduct(ProductUpdateRequest? categoryRequest);
        /// <summary>
        /// Returns all the categories
        /// </summary>
        /// <returns>All categories from the list as List<CategoryResponse> object</returns>
        Task<List<ProductResponse>> GetAllProducts();
        /// <summary>
        /// Returns category object based on the given categoryId
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>Returns the matching category object</returns>
        Task<ProductResponse> GetProductByID(Guid? categoryId);
        /// <summary>
        /// Returns all the matching category based on the given search string
        /// </summary>
        /// <param name="search"></param>
        /// <returns>Returns list of category object</returns>
        Task<List<ProductResponse>> GetProductBySearch(string? search);
        /// <summary>
        /// Deletes the category with the specified category Id.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns>True, if the category was successfully deleted; otherwise, false.</returns>
        Task<bool> DeleteProduct(Guid categoryId); 
    }
}
