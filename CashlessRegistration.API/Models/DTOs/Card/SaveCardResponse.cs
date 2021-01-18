using System;

namespace CashlessRegistration.API.Models.DTOs.Card
{
    public class SaveCardResponse
    {
        public int CardId { get; private init; }
        public DateTime RegistrationDate { get; private init; }
        public ulong Token { get; private init; }
    }
}