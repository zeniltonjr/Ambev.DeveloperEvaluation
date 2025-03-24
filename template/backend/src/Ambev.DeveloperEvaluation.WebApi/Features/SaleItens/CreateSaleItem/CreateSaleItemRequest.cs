﻿namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItens.CreateSaleItem
{
    public class CreateSaleItemRequest
    {
        public Guid SaleId { get; set; } 

        public Guid ProductId { get; set; } 

        public int Quantity { get; set; }  

        public decimal UnitPrice { get; set; }  

        public decimal Discount { get; set; }
    }
}
