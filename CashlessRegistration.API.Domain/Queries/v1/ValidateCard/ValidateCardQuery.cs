namespace CashlessRegistration.API.Domain.Queries.v1.ValidateCard
{
    public class ValidateCardQuery : AbstractQuery<ValidateCardResult>
    {
        public int CustomerId { get; private init; }
        public int CardId { get; private init; }
        public ulong Token { get; private init; }
        public int CVV { get; private init; }

        public ValidateCardQuery(int customerId, int cardId, ulong token, int cvv)
        {
            CustomerId = customerId;
            CardId = cardId;
            Token = token;
            CVV = cvv;
        }
    }
}