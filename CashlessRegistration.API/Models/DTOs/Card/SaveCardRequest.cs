namespace CashlessRegistration.API.Models.DTOs.Card
{
    public class SaveCardRequest
    {
        public int CustomerId { get; init; }
        public long CardNumber { get; init; }
        public int CVV { get; init; }

        public SaveCardRequest(int customerId, long cardNumber, int cvv)
        {
            CustomerId = customerId;
            CardNumber = cardNumber;
            CVV = cvv;
        }
    }
}