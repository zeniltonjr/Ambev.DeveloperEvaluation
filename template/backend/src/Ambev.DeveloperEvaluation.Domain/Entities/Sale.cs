using Ambev.DeveloperEvaluation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale : BaseEntity
    {
        public string SaleNumber { get; private set; }
        public DateTime SaleDate { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid BranchId { get; private set; }
        public decimal TotalAmount { get; private set; }
        public bool IsCancelled { get; private set; }
        public User Customer { get; set; }
        public Branch Branch { get; set; }
        public List<SaleItem> SaleItens { get; set; }

        public Sale() { }

        public Sale(string saleNumber,
                    DateTime saleDate,
                    string customerId,  
                    string branchId,   
                    List<SaleItem> items)
        {
            SaleNumber = saleNumber ?? throw new ArgumentNullException(nameof(saleNumber));
            SaleDate = saleDate;
            CustomerId = Guid.Parse(customerId);  
            BranchId = Guid.Parse(branchId);      
            SaleItens = items ?? new List<SaleItem>();

            ValidateItems();
            CalculateTotalAmount();
        }

        public void UpdateSale(string saleNumber,
                               string customerId, 
                               string branchId,
                               List<SaleItem> updatedItems)
        {
            if (string.IsNullOrEmpty(saleNumber)) throw new ArgumentNullException(nameof(saleNumber));
            if (updatedItems == null) throw new ArgumentNullException(nameof(updatedItems));

            SaleNumber = saleNumber;
            CustomerId = Guid.Parse(customerId);
            BranchId = Guid.Parse(branchId);
            SaleItens = updatedItems;

            CalculateTotalAmount();
            ValidateItems();
        }

        public void CalculateTotalAmount()
        {
            TotalAmount = SaleItens.Sum(item => item.TotalPrice);
        }

        private void ValidateItems()
        {
            foreach (var item in SaleItens)
            {
                if (item.Quantity > 20)
                {
                    throw new DomainException($"Não é possível vender mais de 20 unidades do item {item.ProductId}.");
                }
            }
        }

        public void CancelSale()
        {
            IsCancelled = true;
        }

        public void SetDate(DateTime date)
        {
            SaleDate = date;
        }
    }
}
