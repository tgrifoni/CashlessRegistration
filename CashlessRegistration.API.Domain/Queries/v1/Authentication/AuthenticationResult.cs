using CashlessRegistration.API.Domain.Models.Enums;

namespace CashlessRegistration.API.Domain.Queries.v1.Authentication
{
    public class AuthenticationResult
    {
        public bool IsAuthenticated { get; private init; }
        public AuthenticationStatus AuthenticationStatus { get; private init; }
        public string Token { get; private init; }

        public AuthenticationResult(bool isAuthenticated, AuthenticationStatus authenticationStatus, string token)
        {
            IsAuthenticated = isAuthenticated;
            AuthenticationStatus = authenticationStatus;
            Token = token;
        }
    }
}