using AutoMapper;
using CashlessRegistration.API.Controllers.v1;
using CashlessRegistration.API.Domain.Commands.v1.SaveCard;
using CashlessRegistration.API.Domain.Queries.v1.ValidateCard;
using CashlessRegistration.API.Models.DTOs.Card;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CashlessRegistration.API.Tests.Controllers.v1
{
    public class CardControllerTests
    {
        private readonly Mock<ILogger<CardController>> _loggerMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IMediator> _mediatorMock = new();
        private readonly CardController _cardController;

        public CardControllerTests()
        {
            _cardController = new(_loggerMock.Object, _mapperMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public async Task Save_WhenRequestIsValid_ReturnsCreated()
        {
            _mapperMock
                .Setup(mapper => mapper.Map<SaveCardCommand>(It.IsAny<SaveCardRequest>()))
                .Returns(It.IsAny<SaveCardCommand>());

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<IRequest<SaveCardResult>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<SaveCardResult>());

            _mapperMock
                .Setup(mapper => mapper.Map<SaveCardResponse>(It.IsAny<SaveCardResult>()))
                .Returns(new SaveCardResponse());

            var createdResult = await _cardController.Save(It.IsAny<SaveCardRequest>()) as CreatedResult;

            Assert.NotNull(createdResult);
            Assert.Equal(createdResult.StatusCode, StatusCodes.Status201Created);
            Assert.NotEmpty(createdResult.Location);
            Assert.True(createdResult.Value is SaveCardResponse);
        }

        [Fact]
        public async Task Save_WhenExceptionIsThrown_ReturnsInternalServerError()
        {
            var exceptionMessage = "An error occurred.";
            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<IRequest<SaveCardResult>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new(exceptionMessage));

            var response = await _cardController.Save(It.IsAny<SaveCardRequest>()) as ObjectResult;

            Assert.NotNull(response);
            Assert.Equal(response.StatusCode, StatusCodes.Status500InternalServerError);
            Assert.True(response.Value is Exception);
            Assert.Equal(exceptionMessage, (response.Value as Exception).Message);
        }

        [Fact]
        public async Task Validate_WhenRequestIsValid_ReturnsOk()
        {
            _mapperMock
                .Setup(mapper => mapper.Map<ValidateCardQuery>(It.IsAny<ValidateCardRequest>()))
                .Returns(It.IsAny<ValidateCardQuery>());

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<IRequest<ValidateCardResult>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<ValidateCardResult>());

            _mapperMock
                .Setup(mapper => mapper.Map<ValidateCardResponse>(It.IsAny<ValidateCardResult>()))
                .Returns(It.IsAny<ValidateCardResponse>());

            var response = await _cardController.Validate(It.IsAny<ValidateCardRequest>()) as OkObjectResult;

            Assert.NotNull(response);
            Assert.Equal(response.StatusCode, StatusCodes.Status200OK);
            Assert.True(response.Value is ValidateCardResponse);
        }

        [Fact]
        public async Task Validate_WhenExceptionIsThrown_ReturnsInternalServerError()
        {
            var exceptionMessage = "An error occurred.";
            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<IRequest<ValidateCardResult>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new(exceptionMessage));

            var response = await _cardController.Validate(It.IsAny<ValidateCardRequest>()) as ObjectResult;

            Assert.NotNull(response);
            Assert.Equal(response.StatusCode, StatusCodes.Status500InternalServerError);
            Assert.True(response.Value is Exception);
            Assert.Equal(exceptionMessage, (response.Value as Exception).Message);
        }
    }
}
