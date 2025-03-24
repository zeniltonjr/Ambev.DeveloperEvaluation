using Ambev.DeveloperEvaluation.Application.SaleItens.GetSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.DeleteUser;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.SaleProfile
{
    public class GetSalesRequestProfile : Profile
    {
        public GetSalesRequestProfile()
        {
            CreateMap<GetSalesQueryRequest, GetSalesQueryCommand>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<Sale, GetSaleResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SaleNumber, opt => opt.MapFrom(src => src.SaleNumber))
                .ForMember(dest => dest.SaleDate, opt => opt.MapFrom(src => src.SaleDate))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => src.IsCancelled))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.SaleItens));

            CreateMap<SaleItem, GetSaleItemResult>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice));

            CreateMap<SaleItem, GetSaleItemSaleRequest>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice));

            CreateMap<GetSaleResult, GetSaleResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SaleNumber, opt => opt.MapFrom(src => src.SaleNumber))
                .ForMember(dest => dest.SaleDate, opt => opt.MapFrom(src => src.SaleDate))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.IsCancelled, opt => opt.MapFrom(src => src.IsCancelled))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<GetSalesQueryRequest, GetSaleCommand>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<DeleteSaleRequest, DeleteSaleCommand>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<UpdateSaleRequest, UpdateSaleCommand>()
          .ForMember(dest => dest.SaleId, opt => opt.Ignore()) 
          .ForMember(dest => dest.SaleNumber, opt => opt.MapFrom(src => src.SaleNumber))
          .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
          .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
          .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<SaleItemRequest, SaleItemCommand>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Price));

            CreateMap<UpdateSaleRequest, UpdateSaleCommand>()
           .ConstructUsing(src => new UpdateSaleCommand(
               src.SaleId,
               src.SaleNumber,
               src.CustomerId,
               src.BranchId,
               src.Items.Select(item => new SaleItemCommand(item.ProductId, item.Quantity, item.Price)).ToList()));

            CreateMap<UpdateSaleRequest, UpdateSaleCommand>()
           .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items)); // Mapeando Items diretamente

            // Mapeamento de SaleItemRequest para SaleItemCommand
            CreateMap<SaleItemRequest, SaleItemCommand>();

        }
    }
}
