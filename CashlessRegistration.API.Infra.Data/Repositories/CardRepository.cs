using CashlessRegistration.API.Domain.Contracts.Models.Entities;
using CashlessRegistration.API.Domain.Contracts.Repositories;
using CashlessRegistration.API.Domain.Models.Entities;
using CashlessRegistration.API.Infra.Data.Connections;
using CashlessRegistration.API.Infra.Data.Models.Exceptions;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;

namespace CashlessRegistration.API.Infra.Data.Repositories
{
    public class CardRepository : ICardRepository
    {
        private const string SaveCommand =
            @"INSERT INTO [Card] ([CustomerId], [Number], [RegistrationDate]) VALUES (@CustomerId, @CardNumber, @RegistrationDate);
              SELECT last_insert_rowid();";
        private const string GetByIdQuery =
            @"SELECT [Id], [CustomerId], [Number] AS [CardNumber], [RegistrationDate] FROM [Card] WHERE [Id] = @Id";

        private readonly IConnection _connection;

        public CardRepository(IConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> SaveAsync(ICard card)
        {
            try
            {
                using var connection = _connection.CreateConnection();
                return await connection.ExecuteScalarAsync<int>(SaveCommand, card);
            }
            catch (SqliteException ex)
            {
                var uniqueConstraintErrorCode = 19;
                if (ex.SqliteErrorCode == uniqueConstraintErrorCode)
                    throw SqliteUniqueConstraintException.New;
                throw;
            }
        }

        public async Task<ICard> GetById(int id)
        {
            using var connection = _connection.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Card>(GetByIdQuery, new { Id = id });
        }
    }
}
