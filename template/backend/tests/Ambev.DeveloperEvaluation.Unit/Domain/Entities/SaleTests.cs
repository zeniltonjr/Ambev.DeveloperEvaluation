using System;
using System.Collections.Generic;
using Xunit;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.Tests
{
    public class SaleTests
    {
        [Fact(DisplayName = "Sale should be created with valid parameters")]
        public void Given_ValidParameters_When_CreatingSale_Then_SaleShouldBeCreated()
        {
            // Arrange
            var saleNumber = "S12345";
            var saleDate = DateTime.Now;
            var customerId = Guid.NewGuid().ToString();
            var branchId = Guid.NewGuid().ToString();
            var saleItems = new List<SaleItem>
            {
                new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 5, 50m),
                new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 2, 20m)
            };

            // Act
            var sale = new Sale(saleNumber, saleDate, customerId, branchId, saleItems);

            // Assert
            Assert.NotNull(sale);
            Assert.Equal(saleNumber, sale.SaleNumber);
            Assert.Equal(customerId, sale.CustomerId.ToString());
            Assert.Equal(branchId, sale.BranchId.ToString());
            Assert.Equal(saleItems.Count, sale.SaleItens.Count);
            Assert.Equal(saleItems[0].TotalPrice + saleItems[1].TotalPrice, sale.TotalAmount);
        }

        [Fact(DisplayName = "Sale should be cancellable")]
        public void Given_Sale_When_CancelSale_Then_SaleShouldBeCancelled()
        {
            // Arrange
            var saleNumber = "S12345";
            var saleDate = DateTime.Now;
            var customerId = Guid.NewGuid().ToString();
            var branchId = Guid.NewGuid().ToString();
            var saleItems = new List<SaleItem>
            {
                new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 5, 50m),
                new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 2, 20m)
            };

            var sale = new Sale(saleNumber, saleDate, customerId, branchId, saleItems);

            // Act
            sale.CancelSale();

            // Assert
            Assert.True(sale.IsCancelled);
        }

        [Fact(DisplayName = "Sale should be updated with new data")]
        public void Given_UpdatedSaleData_When_UpdateSale_Then_SaleShouldBeUpdated()
        {
            // Arrange
            var saleNumber = "S12345";
            var saleDate = DateTime.Now;
            var customerId = Guid.NewGuid().ToString();
            var branchId = Guid.NewGuid().ToString();
            var saleItems = new List<SaleItem>
            {
                new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 5, 50m),
                new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 2, 20m)
            };

            var sale = new Sale(saleNumber, saleDate, customerId, branchId, saleItems);

            // New updated data
            var updatedSaleNumber = "S12346";
            var updatedSaleItems = new List<SaleItem>
            {
                new SaleItem(Guid.NewGuid(), Guid.NewGuid(), 10, 30m)
            };

            // Act
            sale.UpdateSale(updatedSaleNumber, customerId, branchId, updatedSaleItems);

            // Assert
            Assert.Equal(updatedSaleNumber, sale.SaleNumber);
            Assert.Equal(updatedSaleItems.Count, sale.SaleItens.Count);
            Assert.Equal(updatedSaleItems[0].TotalPrice, sale.TotalAmount);
        }
    }
}
