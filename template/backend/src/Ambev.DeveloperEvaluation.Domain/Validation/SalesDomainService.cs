using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.Validation
{
    public class SalesDomainService
    {
        public static void ValidateSale(Sale sale)
        {
            foreach (var item in sale.SaleItens)
            {
                if (item.Quantity > 20)
                    throw new BusinessException($"O item '{item.ProductId}' não pode ter mais de 20 unidades.");
            }
        }
    }
}
