using System;

namespace CashlessRegistration.API.Infra.Data.Models.Exceptions
{
    public class SqliteUniqueConstraintException : Exception
    {
        private const string ErrorMessage = "It already exists a record for given Customer and Card Number.";

        private SqliteUniqueConstraintException() : base(ErrorMessage) { }

        public static SqliteUniqueConstraintException New { get => new(); }
    }
}
