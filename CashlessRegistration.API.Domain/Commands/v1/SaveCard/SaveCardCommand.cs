using MediatR;

namespace CashlessRegistration.API.Domain.Commands.v1.SaveCard
{
    public class SaveCardCommand : IRequest<SaveCardResult>
    {
        public int CustomerId { get; private init; }
        public long CardNumber { get; private init; }
        public int CVV { get; private init; }

        public SaveCardCommand(int customerId, long cardNumber, int cvv)
        {
            CustomerId = customerId;
            CardNumber = cardNumber;
            CVV = cvv;
        }
    }
}