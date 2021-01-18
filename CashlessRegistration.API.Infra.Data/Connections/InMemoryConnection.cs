using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;

namespace CashlessRegistration.API.Infra.Data.Connections
{
    public class InMemoryConnection : IConnection, IDisposable
    {
        private const string CreateDatabaseCommand =
            @"CREATE TABLE [Card]
              (
	              [Id] 			     INTEGER PRIMARY KEY AUTOINCREMENT,
	              [CustomerId] 	     INTEGER NOT NULL,
	              [Number]		     BIGINT	NOT NULL,
                  [RegistrationDate] DATETIME NOT NULL,
                  UNIQUE([CustomerId], [Number])
              )";

        private readonly string _connectionString;
        private readonly IDbConnection _masterConnection;

        public InMemoryConnection(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:InMemory"];

            _masterConnection = CreateConnection();
            InitializeDatabase();
        }

        public IDbConnection CreateConnection() =>
            new SqliteConnection(_connectionString);

        private void InitializeDatabase()
        {
            _masterConnection.Open();
            _masterConnection.Execute(CreateDatabaseCommand);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) =>
            _masterConnection?.Close();
    }
}
