using CashlessRegistration.API.Models.DTOs.Card;
using FluentValidation.TestHelper;
using Xunit;

namespace CashlessRegistration.API.Tests.Models.DTOs.Card
{
    public class SaveCardValidatorTests
    {
        private readonly SaveCardRequestValidator _saveCardRequestValidator = new();

        [Theory]
        [InlineData(1, 0L, 0)]
        [InlineData(1, 0L, 99_999)]
        [InlineData(1, 9999_9999_9999_9999, 0)]
        [InlineData(1, 9999_9999_9999_9999, 99_999)]
        public void Validate_WhenRequestIsValid_ShouldNotHaveAnyValidationErrors(int validCustomerId, long validCardNumber, int validCVV)
        {
            var saveCardRequest = new SaveCardRequest(customerId: validCustomerId, cardNumber: validCardNumber, cvv: validCVV);

            var testValidationResult = _saveCardRequestValidator.TestValidate(saveCardRequest);

            testValidationResult.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Validate_WhenCustomerIdIsEmpty_ShouldHaveValidationErrorForCustomerId(int invalidCustomerId)
        {
            var validCardNumber = 0L;
            var validCVV = 0;
            var saveCardRequest = new SaveCardRequest(customerId: invalidCustomerId, cardNumber: validCardNumber, cvv: validCVV);

            var testValidationResult = _saveCardRequestValidator.TestValidate(saveCardRequest);

            testValidationResult.ShouldHaveValidationErrorFor(saveCardRequest => saveCardRequest.CustomerId);
            testValidationResult.ShouldNotHaveValidationErrorFor(saveCardRequest => saveCardRequest.CardNumber);
            testValidationResult.ShouldNotHaveValidationErrorFor(saveCardRequest => saveCardRequest.CVV);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(9_9999_9999_9999_9999)]
        public void Validate_WhenCardNumberIsInvalid_ShouldHaveValidationErrorForCardNumber(long invalidCardNumber)
        {
            var validCustomerId = 1;
            var validCVV = 0;
            var saveCardRequest = new SaveCardRequest(customerId: validCustomerId, cardNumber: invalidCardNumber, cvv: validCVV);

            var testValidationResult = _saveCardRequestValidator.TestValidate(saveCardRequest);

            testValidationResult.ShouldHaveValidationErrorFor(authenticationRequest => authenticationRequest.CardNumber);
            testValidationResult.ShouldNotHaveValidationErrorFor(saveCardRequest => saveCardRequest.CustomerId);
            testValidationResult.ShouldNotHaveValidationErrorFor(saveCardRequest => saveCardRequest.CVV);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(999_999)]
        public void Validate_WhenCVVIsInvalid_ShouldHaveValidationErrorForCVV(int invalidCVV)
        {
            var validCustomerId = 1;
            var validCardNumber = 0L;
            var saveCardRequest = new SaveCardRequest(customerId: validCustomerId, cardNumber: validCardNumber, cvv: invalidCVV);

            var testValidationResult = _saveCardRequestValidator.TestValidate(saveCardRequest);

            testValidationResult.ShouldHaveValidationErrorFor(authenticationRequest => authenticationRequest.CVV);
            testValidationResult.ShouldNotHaveValidationErrorFor(saveCardRequest => saveCardRequest.CustomerId);
            testValidationResult.ShouldNotHaveValidationErrorFor(saveCardRequest => saveCardRequest.CardNumber);
        }
    }
}
