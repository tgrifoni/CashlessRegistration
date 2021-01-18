using CashlessRegistration.API.Models.DTOs.Authentication;
using FluentValidation.TestHelper;
using Xunit;

namespace CashlessRegistration.API.Tests.Models.DTOs.Authentication
{
    public class AuthenticationRequestValidatorTests
    {
        private readonly AuthenticationRequestValidator _authenticationRequestValidator = new();

        [Fact]
        public void Validate_WhenRequestIsValid_ShouldNotHaveAnyValidationErrors()
        {
            var authenticationRequest = new AuthenticationRequest(username: "not empty", password: "not empty");

            var testValidationResult = _authenticationRequestValidator.TestValidate(authenticationRequest);

            testValidationResult.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validate_WhenUsernameIsEmpty_ShouldHaveValidationErrorForUsername()
        {
            var authenticationRequest = new AuthenticationRequest(username: string.Empty, password: "not empty");

            var testValidationResult = _authenticationRequestValidator.TestValidate(authenticationRequest);

            testValidationResult.ShouldHaveValidationErrorFor(authenticationRequest => authenticationRequest.Username);
            testValidationResult.ShouldNotHaveValidationErrorFor(authenticationRequest => authenticationRequest.Password);
        }

        [Fact]
        public void Validate_WhenUsernameIsAllBlankSpaces_ShouldHaveValidationErrorForUsername()
        {
            var authenticationRequest = new AuthenticationRequest(username: "                ", password: "not empty");

            var testValidationResult = _authenticationRequestValidator.TestValidate(authenticationRequest);

            testValidationResult.ShouldHaveValidationErrorFor(authenticationRequest => authenticationRequest.Username);
            testValidationResult.ShouldNotHaveValidationErrorFor(authenticationRequest => authenticationRequest.Password);
        }

        [Fact]
        public void Validate_WhenPasswordIsEmpty_ShouldHaveValidationErrorForPassword()
        {
            var authenticationRequest = new AuthenticationRequest(username: "not empty", password: string.Empty);

            var testValidationResult = _authenticationRequestValidator.TestValidate(authenticationRequest);

            testValidationResult.ShouldHaveValidationErrorFor(authenticationRequest => authenticationRequest.Password);
            testValidationResult.ShouldNotHaveValidationErrorFor(authenticationRequest => authenticationRequest.Username);
        }

        [Fact]
        public void Validate_WhenPasswordIsAllBlankSpaces_ShouldHaveValidationErrorForPassword()
        {
            var authenticationRequest = new AuthenticationRequest(username: "not empty", password: "                ");

            var testValidationResult = _authenticationRequestValidator.TestValidate(authenticationRequest);

            testValidationResult.ShouldHaveValidationErrorFor(authenticationRequest => authenticationRequest.Password);
            testValidationResult.ShouldNotHaveValidationErrorFor(authenticationRequest => authenticationRequest.Username);
        }
    }
}
