using AutoMapper;
using CashlessRegistration.API.Domain.Contracts.Repositories;
using CashlessRegistration.API.Domain.Models.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CashlessRegistration.API.Domain.Commands.v1.SaveCard
{
    public class CardCommandHandler : IRequestHandler<SaveCardCommand, SaveCardResult>
    {
        private readonly ICardRepository _cardRepository;
        private readonly IMapper _mapper;

        public CardCommandHandler(ICardRepository cardRepository, IMapper mapper)
        {
            _cardRepository = cardRepository;
            _mapper = mapper;
        }

        public async Task<SaveCardResult> Handle(SaveCardCommand command, CancellationToken cancellationToken)
        {
            var card = _mapper.Map<Card>(command);
            var id = await _cardRepository.SaveAsync(card);

            card.GenerateToken(command.CVV);

            var result = new SaveCardResult(cardId: id, registrationDate: card.RegistrationDate, token: card.Token);

            return result;
        }
    }
}