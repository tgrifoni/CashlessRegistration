using CashlessRegistration.API.Domain.Contracts.Models.Entities;
using System;
using System.Linq;

namespace CashlessRegistration.API.Domain.Models.Entities
{
    public class Card : ICard
    {
        private const int NumberOfDigits = 4;
        private const double MaxDifferenceInMinutes = 30;

        public int Id { get; private init; }
        public int CustomerId { get; private init; }
        public DateTime RegistrationDate { get; } = DateTime.UtcNow;
        public long CardNumber { get; private init; }
        public ulong Token { get; private set; }

        public bool IsExpired { get => IsRegistrationDateExpired(); }

        protected Card() { }
        public Card(int id, int customerId, long cardNumber)
        {
            Id = id;
            CustomerId = customerId;
            CardNumber = cardNumber;
        }

        public bool IsRegistrationDateExpired()
        {
            var registrationDifferenceInMinutes = (DateTime.UtcNow - RegistrationDate).TotalMinutes;
            return registrationDifferenceInMinutes > MaxDifferenceInMinutes;
        }

        public void GenerateToken(int cvv)
        {
            var lastCardDigitsString = CardNumber.ToString().TakeLast(NumberOfDigits);
            var lastCardDigits = lastCardDigitsString.Select(digit => (int)char.GetNumericValue(digit)).ToArray();
            Token = ProcessToken(lastCardDigits, cvv);
        }

        private ulong ProcessToken(int[] lastCardDigitsArray, int numberOfRotations)
        {
            var lastCardDigitsArrayLength = lastCardDigitsArray.Length;
            var backupArray = new int[lastCardDigitsArrayLength];

            for (int index = 0; index < lastCardDigitsArrayLength; index++)
                backupArray[(index + numberOfRotations % lastCardDigitsArrayLength) % lastCardDigitsArrayLength] = lastCardDigitsArray[index];

            var stringToken = string.Join(string.Empty, backupArray);
            var token = Convert.ToUInt64(stringToken);
            return token;
        }
    }
}
