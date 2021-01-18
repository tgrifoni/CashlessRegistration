using FluentValidation;

namespace CashlessRegistration.API.Models.DTOs.Card
{
    public class SaveCardRequestValidator : AbstractValidator<SaveCardRequest>
    {
        public SaveCardRequestValidator()
        {
            RuleFor(request => request.CustomerId).GreaterThan(0);
            RuleFor(request => request.CardNumber).InclusiveBetween(from: 0, to: 9999_9999_9999_9999);
            RuleFor(request => request.CVV).InclusiveBetween(from: 0, to: 99_999);
        }
    }
}