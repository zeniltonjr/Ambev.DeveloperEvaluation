using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using System;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleItemTests
    {
        /// <summary>
        /// Tests that a SaleItem is created successfully with valid parameters.
        /// </summary>
        [Fact(DisplayName = "SaleItem should be created with valid parameters")]
        public void Given_ValidParameters_When_CreatingSaleItem_Then_SaleItemShouldBeCreated()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var quantity = 5;
            var unitPrice = 50.0m;

            // Act
            var saleItem = new SaleItem(saleId, productId, quantity, unitPrice);

            // Assert
            Assert.NotNull(saleItem);
            Assert.Equal(saleId, saleItem.SaleId);
            Assert.Equal(productId, saleItem.ProductId);
            Assert.Equal(quantity, saleItem.Quantity);
            Assert.Equal(unitPrice, saleItem.UnitPrice);

            var expectedDiscount = unitPrice * quantity * 0.1m;  
            Assert.Equal(expectedDiscount, saleItem.Discount);   

            var expectedTotalPrice = (unitPrice * quantity) - expectedDiscount;
            Assert.Equal(expectedTotalPrice, saleItem.TotalPrice); 
        }

        /// <summary>
        /// Tests that a SaleItem throws an exception if quantity is greater than 20.
        /// </summary>
        [Fact(DisplayName = "SaleItem should throw exception if quantity is greater than 20")]
        public void Given_QuantityGreaterThan20_When_CreatingSaleItem_Then_ShouldThrowBusinessException()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var quantity = 21;
            var unitPrice = 50.0m;

            // Act & Assert
            var exception = Assert.Throws<BusinessException>(() => new SaleItem(saleId, productId, quantity, unitPrice));
            Assert.Equal("Não é permitido vender mais de 20 unidades do mesmo item.", exception.Message);
        }

        /// <summary>
        /// Tests that the discount is correctly applied when quantity is between 10 and 20.
        /// </summary>
        [Fact(DisplayName = "SaleItem should apply correct discount when quantity is between 10 and 20")]
        public void Given_QuantityBetween10And20_When_CreatingSaleItem_Then_ShouldApplyDiscount()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var quantity = 15;
            var unitPrice = 50.0m;

            // Act
            var saleItem = new SaleItem(saleId, productId, quantity, unitPrice);

            // Assert
            var expectedDiscount = unitPrice * quantity * 0.2m; 
            var expectedTotalPrice = (unitPrice * quantity) - expectedDiscount;

            Assert.Equal(expectedDiscount, saleItem.Discount);
            Assert.Equal(expectedTotalPrice, saleItem.TotalPrice);
        }

        /// <summary>
        /// Tests that the discount is correctly applied when quantity is between 4 and 9.
        /// </summary>
        [Fact(DisplayName = "SaleItem should apply correct discount when quantity is between 4 and 9")]
        public void Given_QuantityBetween4And9_When_CreatingSaleItem_Then_ShouldApplyDiscount()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var quantity = 5;
            var unitPrice = 50.0m;

            // Act
            var saleItem = new SaleItem(saleId, productId, quantity, unitPrice);

            // Assert
            var expectedDiscount = unitPrice * quantity * 0.1m; 
            var expectedTotalPrice = (unitPrice * quantity) - expectedDiscount;

            Assert.Equal(expectedDiscount, saleItem.Discount);
            Assert.Equal(expectedTotalPrice, saleItem.TotalPrice);
        }

        /// <summary>
        /// Tests that the discount is zero when quantity is less than 4.
        /// </summary>
        [Fact(DisplayName = "SaleItem should have no discount when quantity is less than 4")]
        public void Given_QuantityLessThan4_When_CreatingSaleItem_Then_ShouldHaveNoDiscount()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var quantity = 3;
            var unitPrice = 50.0m;

            // Act
            var saleItem = new SaleItem(saleId, productId, quantity, unitPrice);

            // Assert
            Assert.Equal(0, saleItem.Discount); 
            Assert.Equal(unitPrice * quantity, saleItem.TotalPrice);
        }

        /// <summary>
        /// Tests that an exception is thrown when updating the quantity to zero or less.
        /// </summary>
        [Fact(DisplayName = "SaleItem should throw exception when updating quantity to zero or less")]
        public void Given_QuantityZeroOrLess_When_UpdatingQuantity_Then_ShouldThrowBusinessException()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var quantity = 5;
            var unitPrice = 50.0m;
            var saleItem = new SaleItem(saleId, productId, quantity, unitPrice);

            // Act & Assert
            var exception = Assert.Throws<BusinessException>(() => saleItem.UpdateQuantity(0));
            Assert.Equal("Quantity must be greater than zero.", exception.Message);
        }

        /// <summary>
        /// Tests that an exception is thrown when updating the unit price to zero or less.
        /// </summary>
        [Fact(DisplayName = "SaleItem should throw exception when updating unit price to zero or less")]
        public void Given_UnitPriceZeroOrLess_When_UpdatingUnitPrice_Then_ShouldThrowBusinessException()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var quantity = 5;
            var unitPrice = 50.0m;
            var saleItem = new SaleItem(saleId, productId, quantity, unitPrice);

            // Act & Assert
            var exception = Assert.Throws<BusinessException>(() => saleItem.UpdateUnitPrice(0));
            Assert.Equal("Unit price must be greater than zero.", exception.Message);
        }

        /// <summary>
        /// Tests that the total price is updated correctly when updating the unit price.
        /// </summary>
        [Fact(DisplayName = "SaleItem should update total price when unit price is updated")]
        public void Given_NewUnitPrice_When_UpdatingUnitPrice_Then_ShouldUpdateTotalPrice()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var quantity = 5;
            var unitPrice = 50.0m;
            var saleItem = new SaleItem(saleId, productId, quantity, unitPrice);

            // Act
            var newUnitPrice = 60.0m;
            saleItem.UpdateUnitPrice(newUnitPrice);

            // Assert
            Assert.Equal(newUnitPrice, saleItem.UnitPrice);
            Assert.Equal((newUnitPrice * quantity) - saleItem.Discount, saleItem.TotalPrice);
        }
    }
}
