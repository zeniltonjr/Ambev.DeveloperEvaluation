using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdatePudct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdatePuduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.ProductProfile
{
    public class UpdateProductRequestProfile : Profile
    {
        public UpdateProductRequestProfile()
        {
            CreateMap<UpdateProductRequest, UpdateProductCommand>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.BasePrice, opt => opt.MapFrom(src => src.BasePrice))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId));

            CreateMap<UpdateProductResult, UpdateProductResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.BasePrice, opt => opt.MapFrom(src => src.BasePrice))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId))
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message));

            CreateMap<IEnumerable<UpdateProductResult>, IEnumerable<UpdateProductResponse>>();

            CreateMap<UpdateProductCommand, UpdateProductResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.BasePrice, opt => opt.MapFrom(src => src.BasePrice))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId));
        }
    }
}
