using CashlessRegistration.API.Domain.Models.Entities;
using Moq;
using System;
using Xunit;

namespace CashlessRegistration.API.Domain.Tests.Models.Entities
{
    public class CardTests
    {
        [Fact]
        public void Card_WhenInitialized_ShouldHaveRegistrationDateSet()
        {
            var card = new Card(id: It.IsAny<int>(), customerId: It.IsAny<int>(), cardNumber: It.IsAny<long>());

            Assert.Equal(DateTime.UtcNow.Date, card.RegistrationDate.Date);
            Assert.Equal(DateTimeKind.Utc, card.RegistrationDate.Kind);
        }

        [Theory]
        [InlineData(1234_5678_9012_3456, 0, 3456U)]
        [InlineData(1234_5678_9012_3456, 1, 6345U)]
        [InlineData(1234_5678_9012_3456, 2, 5634U)]
        [InlineData(1234_5678_9012_3456, 3, 4563U)]
        [InlineData(1234_5678_9012_3456, 4, 3456U)]
        [InlineData(9999_9999_9999_9999, 0, 9999U)]
        [InlineData(9999_9999_9999_9999, 99_999, 9999U)]
        public void GenerateToken_WhenCalled_ShouldGenerateTokenBasedOnCardNumberAndCVV(long cardNumber, int cvv, ulong expectedToken)
        {
            var card = new Card(id: It.IsAny<int>(), customerId: It.IsAny<int>(), cardNumber: cardNumber);

            card.GenerateToken(cvv: cvv);

            Assert.NotEqual(ulong.MinValue, card.Token);
            Assert.Equal(expectedToken, card.Token);
        }
    }
}
