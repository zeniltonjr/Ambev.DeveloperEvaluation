namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class PaginatedSalesResponse
    {
        public List<GetSaleResponse> Sales { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }

        public PaginatedSalesResponse(List<GetSaleResponse> sales, int totalCount, int currentPage, int pageSize)
        {
            Sales = sales;
            TotalCount = totalCount;
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}
