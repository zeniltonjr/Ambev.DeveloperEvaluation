using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings.UserProfile;

public class CreateUserRequestProfile : Profile
{
    public CreateUserRequestProfile()
    {
        CreateMap<GetUserRequest, GetUserCommand>()
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

        CreateMap<AuthenticateUserRequest, AuthenticateUserCommand>();


        CreateMap<AuthenticateUserResult, AuthenticateUserResponse>()
            .ForMember(dest => dest.Token, opt => opt.MapFrom(src => src.Token))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));

        CreateMap<GetUserResult, GetUserResponse>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
              .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
              .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
              .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
              .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));
    }
}
