using AutoMapper;
using CashlessRegistration.API.Controllers.v1;
using CashlessRegistration.API.Domain.Queries.v1.Authentication;
using CashlessRegistration.API.Models.DTOs.Authentication;
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
    public class AuthenticationControllerTests
    {
        private readonly Mock<ILogger<AuthenticationController>> _loggerMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IMediator> _mediatorMock = new();
        private readonly AuthenticationController _authenticationController;

        public AuthenticationControllerTests()
        {
            _authenticationController = new(_loggerMock.Object, _mapperMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public async Task Authenticate_WhenRequestIsValid_ReturnsOk()
        {
            _mapperMock
                .Setup(mapper => mapper.Map<AuthenticationQuery>(It.IsAny<AuthenticationRequest>()))
                .Returns(It.IsAny<AuthenticationQuery>());

            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<IRequest<AuthenticationResult>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(It.IsAny<AuthenticationResult>());

            _mapperMock
                .Setup(mapper => mapper.Map<AuthenticationResponse>(It.IsAny<AuthenticationResult>()))
                .Returns(new AuthenticationResponse());

            var okObjectResult = await _authenticationController.Authenticate(It.IsAny<AuthenticationRequest>()) as OkObjectResult;

            Assert.NotNull(okObjectResult);
            Assert.Equal(okObjectResult.StatusCode, StatusCodes.Status200OK);
            Assert.True(okObjectResult.Value is AuthenticationResponse);
        }

        [Fact]
        public async Task Authenticate_WhenExceptionIsThrown_ReturnsInternalServerError()
        {
            var exceptionMessage = "An error occurred.";
            _mediatorMock
                .Setup(mediator => mediator.Send(It.IsAny<IRequest<AuthenticationResult>>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new(exceptionMessage));

            var response = await _authenticationController.Authenticate(It.IsAny<AuthenticationRequest>()) as ObjectResult;

            Assert.NotNull(response);
            Assert.Equal(response.StatusCode, StatusCodes.Status500InternalServerError);
            Assert.True(response.Value is Exception);
            Assert.Equal(exceptionMessage, (response.Value as Exception).Message);
        }
    }
}
