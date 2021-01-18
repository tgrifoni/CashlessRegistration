using CashlessRegistration.API.Domain.Contracts.Repositories;
using CashlessRegistration.API.Domain.Contracts.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CashlessRegistration.API.Domain.Queries.v1.ValidateCard
{
    public class CardQueryHandler : IRequestHandler<ValidateCardQuery, ValidateCardResult>
    {
        private readonly ICardRepository _cardRepository;
        private readonly IWriterService _writerService;

        public CardQueryHandler(ICardRepository cardRepository, IWriterService writerService)
        {
            _cardRepository = cardRepository;
            _writerService = writerService;
        }

        public async Task<ValidateCardResult> Handle(ValidateCardQuery query, CancellationToken cancellationToken)
        {
            var card = await _cardRepository.GetById(query.CardId);

            var isCustomerOwner = query.CustomerId == card?.CustomerId;
            if (card is null || card.IsExpired || !isCustomerOwner)
                return ValidateCardResult.NotValid;

            _writerService.WriteLine(card.CardNumber);
            card.GenerateToken(query.CVV);

            if (query.Token == card.Token)
                return ValidateCardResult.Valid;

            return ValidateCardResult.NotValid;
        }
    }
}