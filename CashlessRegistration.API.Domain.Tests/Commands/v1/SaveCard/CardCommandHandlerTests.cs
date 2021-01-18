using AutoMapper;
using CashlessRegistration.API.Domain.Commands.v1.SaveCard;
using CashlessRegistration.API.Domain.Contracts.Repositories;
using CashlessRegistration.API.Domain.Models.Entities;
using MediatR;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CashlessRegistration.API.Domain.Tests.Commands.v1.SaveCard
{
    public class CardCommandHandlerTests
    {
        private readonly Mock<ICardRepository> _cardRepositoryMock = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly IRequestHandler<SaveCardCommand, SaveCardResult> _cardCommandHandler;

        public CardCommandHandlerTests()
        {
            _cardCommandHandler = new CardCommandHandler(_cardRepositoryMock.Object, _mapper.Object);
        }

        [Fact]
        public async Task Handle_WhenCalled_ShouldSaveCardAndReturnResult()
        {
            var card = Mock.Of<Card>();
            var saveCardCommand = new SaveCardCommand(customerId: It.IsAny<int>(), cardNumber: It.IsAny<long>(), cvv: It.IsAny<int>());
            _mapper.Setup(mapper => mapper.Map<Card>(saveCardCommand)).Returns(card);

            var result = await _cardCommandHandler.Handle(saveCardCommand, It.IsAny<CancellationToken>());

            Assert.NotNull(result);
            Assert.True(result is SaveCardResult);
            _cardRepositoryMock.Verify(repo => repo.SaveAsync(card), Times.Once);
        }
    }
}
