using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using System.Text.Json.Serialization;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem : BaseEntity
    { 
        public Guid SaleId { get; set; }
        [JsonIgnore]
        public Sale Sale { get; set; } 

        public Guid ProductId { get; set; }
        [JsonIgnore]
        public Product Product { get; set; } 

        public int Quantity { get; set; } 
        public decimal UnitPrice { get; set; }  
        public decimal Discount { get; set; } 
        public decimal TotalPrice { get; set; } 

        public SaleItem() {}

        public SaleItem(Guid saleId, 
                        Guid productId, 
                        int quantity, 
                        decimal unitPrice)
        {
            SaleId = saleId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
            ApplyDiscount();
            TotalPrice = (UnitPrice * Quantity) - Discount;
        }

        private void ApplyDiscount()
        {
            if (Quantity > 20)
                throw new BusinessException("Não é permitido vender mais de 20 unidades do mesmo item.");

            if (Quantity >= 10)
                Discount = (UnitPrice * Quantity) * 0.2m;
            else if (Quantity >= 4)
                Discount = (UnitPrice * Quantity) * 0.1m;
            else
                Discount = 0;
        }

        public static SaleItem Create(Guid saleId, 
                                      Guid productId, 
                                      int quantity,
                                      decimal unitPrice)
        {
            var saleItem = new SaleItem(saleId, productId, quantity, unitPrice);
            return saleItem;
        }

        public void UpdateQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new BusinessException("Quantity must be greater than zero.");
            Quantity = quantity;
            UpdateTotalPrice();
        }

        public void UpdateUnitPrice(decimal unitPrice)
        {
            if (unitPrice <= 0)
                throw new BusinessException("Unit price must be greater than zero.");
            UnitPrice = unitPrice;
            UpdateTotalPrice();
        }

        public void UpdateDiscount(decimal discount)
        {
            if (discount < 0)
                throw new BusinessException("Discount cannot be negative.");
            Discount = discount;
            UpdateTotalPrice();
        }

        public void UpdateTotalPrice()
        {
            TotalPrice = (UnitPrice * Quantity) - Discount;
        }

        public void UpdateSaleItem(int quantity, decimal unitPrice)
        {
            Quantity = quantity;
            UnitPrice = unitPrice;
            ApplyDiscount(); 
            TotalPrice = (UnitPrice * Quantity) - Discount; 
        }
    }
}
