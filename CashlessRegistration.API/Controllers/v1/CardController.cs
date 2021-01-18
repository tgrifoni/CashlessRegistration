using AutoMapper;
using CashlessRegistration.API.Domain.Commands.v1.SaveCard;
using CashlessRegistration.API.Domain.Queries.v1.ValidateCard;
using CashlessRegistration.API.Infra.Data.Models.Exceptions;
using CashlessRegistration.API.Models.DTOs.Card;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CashlessRegistration.API.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CardController : ControllerBase
    {
        private readonly ILogger<CardController> _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CardController(ILogger<CardController> logger, IMapper mapper, IMediator mediator)
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(SaveCardResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] SaveCardRequest request)
        {
            try
            {
                var command = _mapper.Map<SaveCardCommand>(request);
                var result = await _mediator.Send(command);
                var response = _mapper.Map<SaveCardResponse>(result);

                return Created($"/card/{response.CardId}", response);
            }
            catch (SqliteUniqueConstraintException ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status409Conflict, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        [ProducesResponseType(typeof(ValidateCardResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [HttpPost("[action]")]
        public async Task<IActionResult> Validate([FromBody] ValidateCardRequest request)
        {
            try
            {
                var query = _mapper.Map<ValidateCardQuery>(request);
                var result = await _mediator.Send(query);
                var response = _mapper.Map<ValidateCardResponse>(result);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
    }
}