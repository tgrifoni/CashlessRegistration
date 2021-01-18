using AutoMapper;
using CashlessRegistration.API.Domain.Queries.v1.Authentication;
using CashlessRegistration.API.Models.DTOs.Authentication;

namespace CashlessRegistration.API.Profiles
{
    public class AuthenticationProfile : Profile
    {
        public AuthenticationProfile() : base("AuthenticationProfile")
        {
            CreateMap<AuthenticationRequest, AuthenticationQuery>();
            CreateMap<AuthenticationResult, AuthenticationResponse>();
        }
    }
}
