namespace CashlessRegistration.API.Models.Enums
{
    public enum AuthenticationStatusDto
    {
        NotAuthenticated = 0,
        UserNotFound = 1,
        InvalidCredentials = 2,
        Authenticated = 3
    }
}