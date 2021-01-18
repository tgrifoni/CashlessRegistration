namespace CashlessRegistration.API.Domain.Queries.v1.Authentication
{
    public class AuthenticationQuery : AbstractQuery<AuthenticationResult>
    {
        public string Username { get; private init; }
        public string Password { get; private init; }

        public AuthenticationQuery(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}