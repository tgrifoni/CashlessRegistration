using CashlessRegistration.API.Models.DTOs.Card;
using FluentValidation.TestHelper;
using Xunit;

namespace CashlessRegistration.API.Tests.Models.DTOs.Card
{
    public class ValidateCardRequestValidatorTests
    {
        private readonly ValidateCardRequestValidator _validateCardRequestValidator = new();

        [Theory]
        [InlineData(1, 1, 1U, 0)]
        [InlineData(1, 1, ulong.MaxValue, 0)]
        [InlineData(1, 1, 1U, 99_999)]
        [InlineData(1, 1, ulong.MaxValue, 99_999)]
        public void Validate_WhenRequestIsValid_ShouldNotHaveAnyValidationErrors(int validCustomerId, int validCardId, ulong validToken, int validCVV)
        {
            var validateCardRequest = new ValidateCardRequest(customerId: validCustomerId, cardId: validCardId, token: validToken, cvv: validCVV);

            var testValidationResult = _validateCardRequestValidator.TestValidate(validateCardRequest);

            testValidationResult.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Validate_WhenCustomerIdIsEmpty_ShouldHaveValidationErrorForCustomerId(int invalidCustomerId)
        {
            var validCardId = 1;
            var validToken = 1U;
            var validCVV = 0;
            var validateCardRequest = new ValidateCardRequest(customerId: invalidCustomerId, cardId: validCardId, token: validToken, cvv: validCVV);

            var testValidationResult = _validateCardRequestValidator.TestValidate(validateCardRequest);

            testValidationResult.ShouldHaveValidationErrorFor(saveCardRequest => saveCardRequest.CustomerId);
            testValidationResult.ShouldNotHaveValidationErrorFor(saveCardRequest => saveCardRequest.CardId);
            testValidationResult.ShouldNotHaveValidationErrorFor(saveCardRequest => saveCardRequest.Token);
            testValidationResult.ShouldNotHaveValidationErrorFor(saveCardRequest => saveCardRequest.CVV);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Validate_WhenCardIdIsEmpty_ShouldHaveValidationErrorForCardId(int invalidCardId)
        {
            var validCustomerId = 1;
            var validToken = 1U;
            var validCVV = 0;
            var validateCardRequest = new ValidateCardRequest(customerId: validCustomerId, cardId: invalidCardId, token: validToken, cvv: validCVV);

            var testValidationResult = _validateCardRequestValidator.TestValidate(validateCardRequest);

            testValidationResult.ShouldHaveValidationErrorFor(saveCardRequest => saveCardRequest.CardId);
            testValidationResult.ShouldNotHaveValidationErrorFor(saveCardRequest => saveCardRequest.CustomerId);
            testValidationResult.ShouldNotHaveValidationErrorFor(saveCardRequest => saveCardRequest.Token);
            testValidationResult.ShouldNotHaveValidationErrorFor(saveCardRequest => saveCardRequest.CVV);
        }

        [Fact]
        public void Validate_WhenTokenIsInvalid_ShouldHaveValidationErrorForToken()
        {
            var validCustomerId = 1;
            var validCardId = 1;
            var invalidToken = 0U;
            var validCVV = 0;
            var validateCardRequest = new ValidateCardRequest(customerId: validCustomerId, cardId: validCardId, token: invalidToken, cvv: validCVV);

            var testValidationResult = _validateCardRequestValidator.TestValidate(validateCardRequest);

            testValidationResult.ShouldHaveValidationErrorFor(authenticationRequest => authenticationRequest.Token);
            testValidationResult.ShouldNotHaveValidationErrorFor(saveCardRequest => saveCardRequest.CustomerId);
            testValidationResult.ShouldNotHaveValidationErrorFor(saveCardRequest => saveCardRequest.CardId);
            testValidationResult.ShouldNotHaveValidationErrorFor(saveCardRequest => saveCardRequest.CVV);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(999_999)]
        public void Validate_WhenCVVIsInvalid_ShouldHaveValidationErrorForCVV(int invalidCVV)
        {
            var validCustomerId = 1;
            var validCardId = 1;
            var validToken = 1U;
            var validateCardRequest = new ValidateCardRequest(customerId: validCustomerId, cardId: validCardId, token: validToken, cvv: invalidCVV);

            var testValidationResult = _validateCardRequestValidator.TestValidate(validateCardRequest);

            testValidationResult.ShouldHaveValidationErrorFor(authenticationRequest => authenticationRequest.CVV);
            testValidationResult.ShouldNotHaveValidationErrorFor(saveCardRequest => saveCardRequest.CustomerId);
            testValidationResult.ShouldNotHaveValidationErrorFor(saveCardRequest => saveCardRequest.CardId);
            testValidationResult.ShouldNotHaveValidationErrorFor(saveCardRequest => saveCardRequest.Token);
        }
    }
}
