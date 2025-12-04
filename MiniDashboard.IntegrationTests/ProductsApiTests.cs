using Microsoft.AspNetCore.Mvc.Testing;
using ServiceContracts.DTO;
using System.Net;
using System.Net.Http.Json;

namespace MiniDashboard.IntegrationTests
{
    public class ProductsApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private List<ProductResponse> _productsFromSeeds;
        public ProductsApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();

            _productsFromSeeds = new List<ProductResponse>()
            {
                new ProductResponse
                {
                    ProductID = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    ProductName =  "Lenovo Laptop IdeaPAD"
                },
                new ProductResponse
                {
                    ProductID = Guid.Parse( "22222222-2222-2222-2222-222222222222"),
                    ProductName =  "iPhone 17 Pro Max"
                },
                new ProductResponse
                {
                    ProductID = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    ProductName =  "Sterling Notebook"
                },
                new ProductResponse
                {
                    ProductID = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    ProductName =  "Red Seal Vitamin C"
                },
                new ProductResponse
                {
                    ProductID = Guid.Parse("55555555-5555-5555-5555-555555555555"),
                    ProductName =  "Puffer Jacket Medium"
                }
            };
        }

        //Expected from seeds

        
        [Fact]
        public async Task GetAll_ReturnsOk_WithProductsList()
        {
            // Act: call GET /Products/GetAll
            var response = await _client.GetAsync("/Products/GetAll");

            // Assert: status code
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Deserialize response
            var actualProducts = await response.Content.ReadFromJsonAsync<List<ProductResponse>>();
            Assert.NotNull(actualProducts);

  

            // Compare counts
            Assert.Equal(_productsFromSeeds.Count, actualProducts.Count);

 
            Assert.Equal(_productsFromSeeds, actualProducts);
        }
        [Fact]
        public async Task GetProductByProductID_ReturnsOk_WithValidProductID()
        {
            // Act: call GET /Products/{ProductID}
            var response = await _client.GetAsync("/Products/22222222-2222-2222-2222-222222222222");

            // Assert: status code
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);            

            // Deserialize response
            var product = await response.Content.ReadFromJsonAsync<ProductResponse>();

            // Expected seeded product
            var expectedProduct = _productsFromSeeds.Where(w => w.ProductID == product.ProductID).FirstOrDefault();

            Assert.NotNull(product);         
            Assert.Equal(expectedProduct, product);

        }

        [Fact]
        public async Task GetProductByProductID_ReturnsBadRequest_WithInvalidProductID()
        {
            // Act: call GET /Products/{ProductID}
            var response = await _client.GetAsync("/Products/22222222-2222-2222-2222-222222222223");

            // Assert: status code
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            // Deserialize response
            var product = await response.Content.ReadFromJsonAsync<ProductResponse>();

            // Expected seeded product
            var expectedId = Guid.Parse("22222222-2222-2222-2222-222222222222");
            var expectedName = "iPhone 17 Pro Max";

            Assert.Null(product?.ProductName);
            Assert.NotEqual(expectedId, product?.ProductID);
            Assert.NotEqual(expectedName, product?.ProductName);
        }
        [Fact]
        public async Task GetProductBySearch_ReturnsOk_WithValidSearchString()
        {
            // Act: call GET /Products/Search?query
            var response = await _client.GetAsync("/Products/Search?query=o");

            // Assert: status code
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

           var expectedProducts = _productsFromSeeds.Where(w => w.ProductName.Contains("o"));

            // Deserialize response
            var actualProducts = await response.Content.ReadFromJsonAsync<List<ProductResponse>>();

            Assert.Equal(expectedProducts, actualProducts);
        }

        [Fact]
        public async Task GetProductBySearch_ReturnsOk_WithNullOrEmptyString()
        {
            // Act: call GET /Products/Search?query
            var response = await _client.GetAsync("/Products/Search?query=");

            // Assert: status code
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

           

            // Deserialize response
            var actualProducts = await response.Content.ReadFromJsonAsync<List<ProductResponse>>();

            Assert.Equal(_productsFromSeeds, actualProducts);
        }
        [Fact]
        public async Task CreateProduct_ReturnsOk_WithValidProductName()
        {
            // Arrange: prepare request
            var request = new ProductAddRequest
            {
                ProductName = "Integration Test Product"
            };

            // Act: call POST /Products/Create
            var response = await _client.PostAsJsonAsync("/Products/Create", request);

            // Assert: status code
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Deserialize response
            var product = await response.Content.ReadFromJsonAsync<ProductResponse>();
            Assert.NotNull(product);

            // Assert: product matches request            
            Assert.Equal("Integration Test Product", product.ProductName);
            Assert.NotEqual(Guid.Empty, product.ProductID); // should be auto generated from product service 
        }

        [Fact]
        public async Task UpdateProduct_ReturnsOk_WithValidProductName()
        {
            // Arrange: prepare request
            var request = new ProductUpdateRequest
            {
                ProductID = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                ProductName = "Updated Product Name"
            };
            // Act: call PUT /Products/Update/{productId}
            var response = await _client.PutAsJsonAsync($"/Products/Update/{request.ProductID}", request);
            // Assert: status code
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Deserialize response
            var product = await response.Content.ReadFromJsonAsync<ProductResponse>();
            Assert.NotNull(product);

            // Assert: product matches request            
            Assert.Equal(request.ProductName, product.ProductName);
            Assert.Equal(request.ProductID, product.ProductID);
        }
        [Fact]
        public async Task UpdateProduct_ReturnsOk_WithDuplicateProductName()
        {
            // Arrange: prepare request
            var request = new ProductUpdateRequest
            {
                ProductID = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                ProductName = "iPhone 17 Pro Max"
            };
            // Act: call PUT /Products/Update/{productId}
            var response = await _client.PutAsJsonAsync($"/Products/Update/{request.ProductID}", request);
            // Assert: status code
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }

        [Fact]
        public async Task DeleteProduct_ReturnsTrue_WithValidProductID() 
        {             
            // Arrange: prepare request
            var productId = Guid.Parse("33333333-3333-3333-3333-333333333333");
            // Act: call DELETE /Products/Delete/{productId}

            var response = await _client.DeleteAsync($"/Products/Delete/{productId}");
            // Assert: status code

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            // Deserialize response
            var deleteResult = await response.Content.ReadFromJsonAsync<bool>();
            Assert.True(deleteResult);
        }
        [Fact]
        public async Task DeleteProduct_ReturnsFalse_WithInvalidProductID() 
        {             
            // Arrange: prepare request
            var productId = Guid.NewGuid();

            // Act: call DELETE /Products/Delete/{productId}
            var response = await _client.DeleteAsync($"/Products/Delete/{productId}");

            // Assert: status code
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Deserialize response
            var deleteResult = await response.Content.ReadFromJsonAsync<bool>();
            Assert.False(deleteResult);
        }

    }
}