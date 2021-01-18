using AutoMapper;
using CashlessRegistration.API.Domain.Commands.v1.SaveCard;
using CashlessRegistration.API.Domain.Models.Entities;
using CashlessRegistration.API.Domain.Queries.v1.ValidateCard;
using CashlessRegistration.API.Models.DTOs.Card;

namespace CashlessRegistration.API.Profiles
{
    public class CardProfile : Profile
    {
        public CardProfile() : base("CardProfile")
        {
            CreateMap<SaveCardRequest, SaveCardCommand>();
            CreateMap<SaveCardCommand, Card>()
                .ForMember(card => card.Id, opt => opt.Ignore())
                .ForMember(card => card.Token, opt => opt.Ignore());
            CreateMap<SaveCardResult, SaveCardResponse>();
            CreateMap<ValidateCardRequest, ValidateCardQuery>();
            CreateMap<ValidateCardResult, ValidateCardResponse>();
        }
    }
}
