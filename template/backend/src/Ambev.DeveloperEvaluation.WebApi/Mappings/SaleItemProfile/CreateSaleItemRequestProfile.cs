using Ambev.DeveloperEvaluation.Application.SaleItens.CreateSaleItem;
using Ambev.DeveloperEvaluation.Application.SaleItens.DeleteSaleItem;
using Ambev.DeveloperEvaluation.Application.SaleItens.GetSaleItem;
using Ambev.DeveloperEvaluation.Application.SaleItens.UpdateSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.SaleItens.CreateSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.SaleItens.DeleteSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.SaleItens.GetSaleItem;
using Ambev.DeveloperEvaluation.WebApi.Features.SaleItens.UpdateSaleItem;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.SaleItemProfile
{
    public class CreateSaleItemRequestProfile : Profile
    {
        public CreateSaleItemRequestProfile()
        {
            CreateMap<GetSaleItemRequest, GetSaleItemCommand>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<CreateSaleItemRequest, CreateSaleItemCommand>();

            CreateMap<CreateSaleItemResult, CreateSaleItemResponse>();

            CreateMap<DeleteSaleItemRequest, DeleteSaleItemCommand>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<GetSaleItemResult, GetSaleItemResponse>()
               .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => (src.UnitPrice * src.Quantity)));

            CreateMap<UpdateSaleItemRequest, UpdateSaleItemCommand>()
               .ForMember(dest => dest.SaleItemId, opt => opt.MapFrom(src => src.saleId))
               .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
               .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice));

            CreateMap<UpdateSaleItemResult, UpdateSaleItemResponse>();
        }
    }
}
