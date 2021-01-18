using FluentValidation;

namespace CashlessRegistration.API.Models.DTOs.Authentication
{
    public class AuthenticationRequestValidator : AbstractValidator<AuthenticationRequest>
    {
        public AuthenticationRequestValidator()
        {
            RuleFor(request => request.Username).NotEmpty();
            RuleFor(request => request.Password).NotEmpty();
        }
    }
}