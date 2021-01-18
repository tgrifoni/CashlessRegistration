namespace CashlessRegistration.API.Models.DTOs.Authentication
{
    public class AuthenticationRequest
    {
        public string Username { get; init; }
        public string Password { get; init; }

        public AuthenticationRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}