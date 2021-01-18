using CashlessRegistration.API.Models.Enums;

namespace CashlessRegistration.API.Models.DTOs.Authentication
{
    public class AuthenticationResponse
    {
        public bool IsAuthenticated { get; private init; }
        public AuthenticationStatusDto AuthenticationStatus { get; private init; }
        public string Token { get; private init; }
    }
}