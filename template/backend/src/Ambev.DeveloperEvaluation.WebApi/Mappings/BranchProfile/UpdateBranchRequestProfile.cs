using Ambev.DeveloperEvaluation.Application.Branchs.UpdateBranch;
using Ambev.DeveloperEvaluation.WebApi.Features.Branchs.GetUpdate;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.BranchProfile
{
    public class UpdateBranchRequestProfile : Profile
    {
        public UpdateBranchRequestProfile()
        {
            CreateMap<UpdateBranchResult, UpdateBranchCommand>();
            CreateMap<UpdateBranchResult, UpdateBranchResponse>();
            CreateMap<UpdateBranchRequest, UpdateBranchCommand>();
        }
    }
}
