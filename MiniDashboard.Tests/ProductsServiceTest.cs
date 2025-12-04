using Entities;
using Moq;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;

namespace MiniDashboard.Tests
{
    public class ProductsServiceTest
    {
        private readonly IProductsService _productsService;

        private readonly Mock<IProductsRepository> _productRepositoryMock;
        private readonly IProductsRepository _productRepository;

        public ProductsServiceTest()
        {
            _productRepositoryMock = new Mock<IProductsRepository>();

            _productRepository = _productRepositoryMock.Object;
            _productsService = new ProductsService(_productRepository);
        }

        [Fact]
        public async Task AddProduct_NullProduct_ToBeArgumentNullException()
        {
            //Arrange
            ProductAddRequest? productAddRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _productsService.AddProduct(productAddRequest);
            });
        }

        [Fact]
        public async Task AddProduct_DuplicateProductName_ToBeArgumentException()
        {
            //Arrange
            ProductAddRequest? productAddRequest = new ProductAddRequest()
            {
                ProductName = "iPhone 17 Pro Max"
            };
            _productRepositoryMock.Setup(repo => repo.GetProductByName(It.IsAny<string>()))
                .ReturnsAsync(new Entities.Product()
                {
                    ProductID = Guid.NewGuid(),
                    ProductName = "iPhone 17 Pro Max"
                });
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _productsService.AddProduct(productAddRequest);
            });
        }
        [Fact]
        public async Task AddProduct_ProductNameIsNull_ToBeArgumentException()
        {
            //Arrange
            ProductAddRequest? productAddRequest = new ProductAddRequest()
            {
                ProductName = null
            };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _productsService.AddProduct(productAddRequest);
            });
        }
        [Fact]
        public async Task GetProductByProductID_NullProductID_ToBeArgumentException()
        {
            //Arrange
            Guid? productId = null;

            //Act           
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                ProductResponse? response = await _productsService.GetProductByID(productId);
            });
        }

        [Fact]
        public async Task GetProductByProductID_WithProductID_ToBeSuccessful()
        {
            //Arrange
            Product product = new Product()
            {
                ProductID = Guid.NewGuid(),
                ProductName = "Proper Product"
            };

            ProductResponse productResponseExpected = product.ToProductResponse();

            _productRepositoryMock.Setup(temp => temp.GetProductByID(It.IsAny<Guid>()))
                .ReturnsAsync(product);


            //Act
            ProductResponse responseFromGet = await _productsService.GetProductByID(product.ProductID);

            //Assert
            Assert.Equal(productResponseExpected,responseFromGet);
        }

        [Fact]
        public async Task UpdateProduct_NullProduct_ToBeArgumentNullException()
        {
            //Arrange
            ProductUpdateRequest? productUpdateRequest = null;
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _productsService.UpdateProduct(productUpdateRequest);
            });
        }
        [Fact]
        public async Task UpdateProduct_InvalidProductID_ToBeArgumentException()
        {
            //Arrange
            ProductUpdateRequest? productUpdateRequest = new ProductUpdateRequest()
            {
                ProductID = Guid.NewGuid(),
                ProductName = "Updated Product Name"
            };
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _productsService.UpdateProduct(productUpdateRequest);
            });
        }

        [Fact]
        public async Task DeleteProduct_InvalidProductID_ToBeFalse()
        {
             //Arrange
            Guid productId = Guid.NewGuid();
            _productRepositoryMock.Setup(temp => temp.DeleteProduct(It.IsAny<Guid>()))
                .ReturnsAsync(false);
            //Act
            bool isDeleted =  await _productsService.DeleteProduct(productId);
            //Assert
            Assert.False(isDeleted);
        }
        [Fact]  
        public async Task DeleteProduct_ValidProductID_ToBeTrue()
                    {
            //Arrange
            Guid productId = Guid.NewGuid();
            _productRepositoryMock.Setup(temp => temp.DeleteProduct(It.IsAny<Guid>()))
                .ReturnsAsync(true);
            //Act
            bool isDeleted = await _productsService.DeleteProduct(productId);
            //Assert
            Assert.True(isDeleted);
        }   


    }
}