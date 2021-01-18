using CashlessRegistration.API.Domain.Models.Enums;
using CashlessRegistration.API.Domain.Queries.v1.Authentication;
using MediatR;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CashlessRegistration.API.Domain.Tests.Queries.v1.Authentication
{
    public class AuthenticationQueryHandlerTests
    {
        private readonly Mock<IConfiguration> _configurationMock = new();
        private readonly IRequestHandler<AuthenticationQuery, AuthenticationResult> _authenticationQueryHandler;

        public AuthenticationQueryHandlerTests()
        {
            _authenticationQueryHandler = new AuthenticationQueryHandler(_configurationMock.Object);
        }

        [Fact]
        public async Task Handle_WhenCalled_ShouldReturnJwtToken()
        {
            var issuer = "TestIssuer";
            _configurationMock.SetupGet(configuration => configuration["Jwt:Issuer"]).Returns(issuer);

            var hoursToExpire = 0.01;
            _configurationMock.SetupGet(configuration => configuration["Jwt:HoursToExpire"]).Returns($"{hoursToExpire}");

            var key = "------ ThisIsATestKey ------";
            _configurationMock.SetupGet(configuration => configuration["Jwt:Key"]).Returns(key);

            var username = "username";
            var query = new AuthenticationQuery(username: username, password: It.IsAny<string>());

            var result = await _authenticationQueryHandler.Handle(query, It.IsAny<CancellationToken>());
            var token = new JwtSecurityTokenHandler().ReadJwtToken(result.Token);

            Assert.NotNull(result);
            Assert.True(result.IsAuthenticated);
            Assert.Equal(AuthenticationStatus.Authenticated, result.AuthenticationStatus);
            Assert.Equal(username, token.Claims.Single(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value);
            Assert.Equal(issuer, token.Issuer);
        }
    }
}
