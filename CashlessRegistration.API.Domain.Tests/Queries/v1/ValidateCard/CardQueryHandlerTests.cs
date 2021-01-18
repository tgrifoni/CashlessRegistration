using CashlessRegistration.API.Domain.Contracts.Models.Entities;
using CashlessRegistration.API.Domain.Contracts.Repositories;
using CashlessRegistration.API.Domain.Contracts.Services;
using CashlessRegistration.API.Domain.Queries.v1.ValidateCard;
using MediatR;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CashlessRegistration.API.Domain.Tests.Queries.v1.ValidateCard
{
    public class CardQueryHandlerTests
    {
        private readonly Mock<ICardRepository> _cardRepositoryMock = new();
        private readonly Mock<IWriterService> _writerServiceMock = new();
        private readonly IRequestHandler<ValidateCardQuery, ValidateCardResult> _cardQueryHandler;

        public CardQueryHandlerTests()
        {
            _cardQueryHandler = new CardQueryHandler(_cardRepositoryMock.Object, _writerServiceMock.Object);
        }

        [Fact]
        public async Task Handle_WhenRegistrationTokenIsExpired_ShouldReturnThatCardIsNotValid()
        {
            var cardMock = new Mock<ICard>();
            cardMock.SetupGet(card => card.IsExpired).Returns(true);
            var card = cardMock.Object;
            _cardRepositoryMock.Setup(repository => repository.GetById(card.Id)).ReturnsAsync(card);

            var query = new ValidateCardQuery(cardId: card.Id, customerId: card.CustomerId, cvv: It.IsAny<int>(), token: It.IsAny<ulong>());

            var result = await _cardQueryHandler.Handle(query, It.IsAny<CancellationToken>());

            Assert.False(result.Validated);
        }

        [Fact]
        public async Task Handle_WhenCustomerIdsAreDifferent_ShouldReturnThatCardIsNotValid()
        {
            var cardMock = new Mock<ICard>();
            cardMock.SetupGet(card => card.IsExpired).Returns(false);
            var someCustomerId = 1;
            cardMock.SetupGet(card => card.CustomerId).Returns(someCustomerId);
            var card = cardMock.Object;
            _cardRepositoryMock.Setup(repository => repository.GetById(card.Id)).ReturnsAsync(card);

            var anotherCustomerId = 2;
            var query = new ValidateCardQuery(cardId: card.Id, customerId: anotherCustomerId, cvv: It.IsAny<int>(), token: It.IsAny<ulong>());

            var result = await _cardQueryHandler.Handle(query, It.IsAny<CancellationToken>());

            Assert.False(result.Validated);
            Assert.False(someCustomerId == anotherCustomerId);
        }

        [Fact]
        public async Task Handle_WhenRegistrationDateAndCustomerIdAreValid_ShouldPrintCardNumber()
        {
            var cardMock = new Mock<ICard>();
            cardMock.SetupGet(card => card.IsExpired).Returns(false);
            var card = cardMock.Object;
            _cardRepositoryMock.Setup(repository => repository.GetById(card.Id)).ReturnsAsync(card);

            var query = new ValidateCardQuery(cardId: card.Id, customerId: card.CustomerId, cvv: It.IsAny<int>(), token: card.Token);

            var result = await _cardQueryHandler.Handle(query, It.IsAny<CancellationToken>());

            _writerServiceMock.Verify(writer => writer.WriteLine(card.CardNumber), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenTokenIsValid_ShouldReturnThatCardIsValid()
        {
            var cardMock = new Mock<ICard>();
            cardMock.SetupGet(card => card.IsExpired).Returns(false);
            var expectedToken = It.IsAny<ulong>();
            cardMock.SetupGet(card => card.Token).Returns(expectedToken);
            var card = cardMock.Object;
            _cardRepositoryMock.Setup(repository => repository.GetById(card.Id)).ReturnsAsync(card);

            var query = new ValidateCardQuery(cardId: card.Id, customerId: card.CustomerId, cvv: It.IsAny<int>(), token: card.Token);

            var result = await _cardQueryHandler.Handle(query, It.IsAny<CancellationToken>());

            Assert.True(result.Validated);
        }

        [Fact]
        public async Task Handle_WhenTokenIsNotValid_ShouldReturnThatCardIsNotValid()
        {
            var cardMock = new Mock<ICard>();
            cardMock.SetupGet(card => card.IsExpired).Returns(false);
            var someToken = 1U;
            cardMock.SetupGet(card => card.Token).Returns(someToken);
            var card = cardMock.Object;
            _cardRepositoryMock.Setup(repository => repository.GetById(card.Id)).ReturnsAsync(card);

            var anotherToken = 2U;
            var query = new ValidateCardQuery(cardId: card.Id, customerId: card.CustomerId, cvv: It.IsAny<int>(), token: anotherToken);

            var result = await _cardQueryHandler.Handle(query, It.IsAny<CancellationToken>());

            Assert.False(result.Validated);
            Assert.False(someToken == anotherToken);
        }
    }
}
