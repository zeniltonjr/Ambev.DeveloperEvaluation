using Ambev.DeveloperEvaluation.WebApi.Features.Branchs.GetBranch;
using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Branchs.GetBranch;
using Ambev.DeveloperEvaluation.WebApi.Features.Branchs.CreateBranch;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.Branchs.CreateBranch;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;


namespace Ambev.DeveloperEvaluation.WebApi.Mappings.BranchProfile
{
    public class BranchRequestProfile : Profile
    {
        public BranchRequestProfile()
        {
            CreateMap<CreateBranchRequest, CreateBranchCommand>();
            CreateMap<CreateBranchResult, CreateBranchResponse>();

            CreateMap<Branch, GetBranchResult>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
              .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Products));

            CreateMap<Product, GetProductResult>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.BasePrice, opt => opt.MapFrom(src => src.BasePrice))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BranchId));

            CreateMap<GetBranchRequest, GetBranchCommand>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<GetBranchResult, GetBranchResponse>();
            CreateMap<GetBranchRequest, GetBranchCommand>();
        }
    }
}
