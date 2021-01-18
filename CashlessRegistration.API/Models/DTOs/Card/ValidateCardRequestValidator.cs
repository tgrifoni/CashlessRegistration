using FluentValidation;

namespace CashlessRegistration.API.Models.DTOs.Card
{
    public class ValidateCardRequestValidator : AbstractValidator<ValidateCardRequest>
    {
        public ValidateCardRequestValidator()
        {
            RuleFor(request => request.CustomerId).NotEmpty().GreaterThan(0);
            RuleFor(request => request.CardId).NotEmpty().GreaterThan(0);
            RuleFor(request => request.Token).NotEmpty();
            RuleFor(request => request.CVV).InclusiveBetween(from: 0, to: 99_999);
        }
    }
}