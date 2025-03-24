using Ambev.DeveloperEvaluation.Domain.Entities;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class ProductTests
    {
        /// <summary>
        /// Tests that a product is created successfully with valid parameters.
        /// </summary>
        [Fact(DisplayName = "Product should be created with valid parameters")]
        public void Given_ValidParameters_When_CreatingProduct_Then_ProductShouldBeCreated()
        {
            // Arrange
            var name = "Test Product";
            var basePrice = 99.99m;
            var branchId = Guid.NewGuid();

            // Act
            var product = new Product(name, basePrice, branchId);

            // Assert
            Assert.NotNull(product);
            Assert.Equal(name, product.Name);
            Assert.Equal(basePrice, product.BasePrice);
            Assert.Equal(branchId, product.BranchId);
        }

        /// <summary>
        /// Tests that the product name can be set correctly.
        /// </summary>
        [Fact(DisplayName = "Product name should be set correctly")]
        public void Given_Product_When_SettingName_Then_NameShouldBeUpdated()
        {
            // Arrange
            var product = new Product("Old Name", 99.99m, Guid.NewGuid());
            var newName = "New Name";

            // Act
            product.Name = newName; 

            // Assert
            Assert.Equal(newName, product.Name);
        }

        /// <summary>
        /// Tests that the base price is set correctly.
        /// </summary>
        [Fact(DisplayName = "Product base price should be set correctly")]
        public void Given_Product_When_SettingBasePrice_Then_BasePriceShouldBeUpdated()
        {
            // Arrange
            var product = new Product("Test Product", 99.99m, Guid.NewGuid());
            var newBasePrice = 149.99m;

            // Act
            product.BasePrice = newBasePrice;

            // Assert
            Assert.Equal(newBasePrice, product.BasePrice);
        }

        /// <summary>
        /// Tests that the product is associated with the correct branch ID.
        /// </summary>
        [Fact(DisplayName = "Product should be associated with the correct branch ID")]
        public void Given_Product_When_Initialized_Then_ShouldHaveCorrectBranchId()
        {
            // Arrange
            var branchId = Guid.NewGuid();
            var product = new Product("Test Product", 99.99m, branchId);

            // Act
            var associatedBranchId = product.BranchId;

            // Assert
            Assert.Equal(branchId, associatedBranchId);
        }
    }
}
