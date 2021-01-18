using System;

namespace CashlessRegistration.API.Domain.Contracts.Models.Entities
{
    public interface ICard
    {
        public int Id { get; }
        public int CustomerId { get; }
        public DateTime RegistrationDate { get; }
        public long CardNumber { get; }
        public ulong Token { get; }

        public bool IsExpired { get; }

        void GenerateToken(int cvv);
    }
}
