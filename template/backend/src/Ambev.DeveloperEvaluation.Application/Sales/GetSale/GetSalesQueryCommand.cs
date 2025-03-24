using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSalesQueryCommand : IRequest<PaginatedSalesResponse>
    {
        public Guid Id { get; set; }
        public string? Customer { get; set; }
        public string? Branch { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetSalesQueryCommand(string? customer, string? branch, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize)
        {
            Customer = customer;
            Branch = branch;
            StartDate = startDate;
            EndDate = endDate;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
