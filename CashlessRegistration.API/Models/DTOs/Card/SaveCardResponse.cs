using System;
using System.Text.Json.Serialization;

namespace CashlessRegistration.API.Models.DTOs.Card
{
    public class SaveCardResponse
    {
        [JsonInclude]
        public int CardId { get; private init; }
        [JsonInclude]
        public DateTime RegistrationDate { get; private init; }
        [JsonInclude]
        public ulong Token { get; private init; }
    }
}