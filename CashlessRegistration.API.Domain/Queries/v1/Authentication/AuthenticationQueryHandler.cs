using CashlessRegistration.API.Domain.Models.Enums;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CashlessRegistration.API.Domain.Queries.v1.Authentication
{
    public class AuthenticationQueryHandler : IRequestHandler<AuthenticationQuery, AuthenticationResult>
    {
        private readonly IConfiguration _configuration;

        public AuthenticationQueryHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // TODO: For now, any attempt to login will be authenticated. Need to develop a more sophisticated authentication
        public async Task<AuthenticationResult> Handle(AuthenticationQuery query, CancellationToken cancellationToken) =>
            await Task.FromResult(new AuthenticationResult
            (
                isAuthenticated: true,
                authenticationStatus: AuthenticationStatus.Authenticated,
                token: GetJwtToken(query.Username)
            ));

        private string GetJwtToken(string username)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var hoursToExpire = Convert.ToDouble(_configuration["Jwt:HoursToExpire"]);
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var claims = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            });

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = issuer,
                Issuer = issuer,
                Expires = DateTime.UtcNow.AddHours(hoursToExpire),
                NotBefore = DateTime.UtcNow,
                SigningCredentials = signingCredentials,
                Subject = claims
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}