namespace CashlessRegistration.API.Models.DTOs.Card
{
    public class ValidateCardRequest
    {
        public int CustomerId { get; init; }
        public int CardId { get; init; }
        public ulong Token { get; init; }
        public int CVV { get; init; }

        public ValidateCardRequest(int customerId, int cardId, ulong token, int cvv)
        {
            CustomerId = customerId;
            CardId = cardId;
            Token = token;
            CVV = cvv;
        }
    }
}