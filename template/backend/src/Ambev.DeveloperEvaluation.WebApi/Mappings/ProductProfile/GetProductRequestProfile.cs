using Ambev.DeveloperEvaluation.Application.Branchs.GetBranch;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Features.Branchs.GetBranch;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.ProductProfile
{
    public class GetProductRequestProfile : Profile
    {
        public GetProductRequestProfile()
        {
            CreateMap<GetBranchRequest, GetBranchCommand>();

            CreateMap<GetProductResult, GetProductResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.BasePrice, opt => opt.MapFrom(src => src.BasePrice))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.Id));

            CreateMap<Branch, GetBranchResponse>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

            CreateMap<Product, GetProductResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.BasePrice, opt => opt.MapFrom(src => src.BasePrice))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId));

            CreateMap<Product, GetProductResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.BasePrice, opt => opt.MapFrom(src => src.BasePrice))
                .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => src.BranchId));
        }
    }
}
