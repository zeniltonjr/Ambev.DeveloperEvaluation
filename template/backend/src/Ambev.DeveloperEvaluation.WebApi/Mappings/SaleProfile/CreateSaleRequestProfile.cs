using Ambev.DeveloperEvaluation.Application.SaleItens.CreateSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.SaleProfile
{
    public class CreateSaleRequestProfile : Profile
    {
        public CreateSaleRequestProfile()
        {
            CreateMap<GetSaleRequest, GetSaleCommand>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<CreateSaleRequest, CreateSaleCommand>()
           .ForMember(dest => dest.SaleNumber, opt => opt.MapFrom(src => src.SaleNumber))
           .ForMember(dest => dest.SaleDate, opt => opt.MapFrom(src => src.SaleDate))
           .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
           .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
           .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
           .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => src.IsCancelled))
           .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items)); 

            CreateMap<CreateSaleItemSaleRequest, CreateSaleItemCommand>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Price));

            CreateMap<CreateSaleResult, CreateSaleResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }
    }
}
