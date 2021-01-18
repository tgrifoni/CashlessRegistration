using System;

namespace CashlessRegistration.API.Domain.Commands.v1.SaveCard
{
    public class SaveCardResult
    {
        public int CardId { get; private init; }
        public DateTime RegistrationDate { get; private init; }
        public ulong Token { get; private init; }

        public SaveCardResult(int cardId, DateTime registrationDate, ulong token)
        {
            CardId = cardId;
            RegistrationDate = registrationDate;
            Token = token;
        }
    }
}
