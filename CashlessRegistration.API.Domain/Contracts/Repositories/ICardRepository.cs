using CashlessRegistration.API.Domain.Contracts.Models.Entities;
using System.Threading.Tasks;

namespace CashlessRegistration.API.Domain.Contracts.Repositories
{
    public interface ICardRepository
    {
        Task<int> SaveAsync(ICard card);
        Task<ICard> GetById(int id);
    }
}