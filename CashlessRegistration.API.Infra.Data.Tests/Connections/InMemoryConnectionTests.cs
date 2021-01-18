using CashlessRegistration.API.Infra.Data.Connections;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace CashlessRegistration.API.Infra.Data.Tests.Connections
{
    public class InMemoryConnectionTests
    {
        private const string ConnectionString = "Data Source=:memory:";
        private readonly Mock<IConfiguration> _configurationMock = new();
        private readonly IConnection _connection;

        public InMemoryConnectionTests()
        {
            _configurationMock.SetupGet(configuration => configuration[It.IsAny<string>()]).Returns(ConnectionString);
            _connection = new InMemoryConnection(_configurationMock.Object);
        }

        [Fact]
        public void CreateConnection_WhenCalled_ReturnsSqliteConnection()
        {
            using var connection = _connection.CreateConnection();

            Assert.NotNull(connection);
            Assert.Equal(ConnectionString, connection.ConnectionString);
        }
    }
}
