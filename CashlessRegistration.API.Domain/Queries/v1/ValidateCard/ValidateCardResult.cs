namespace CashlessRegistration.API.Domain.Queries.v1.ValidateCard
{
    public struct ValidateCardResult
    {
        public bool Validated { get; private init; }

        public static ValidateCardResult Valid => new ValidateCardResult { Validated = true };
        public static ValidateCardResult NotValid => new ValidateCardResult { Validated = false };
    }
}