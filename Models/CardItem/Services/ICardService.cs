using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Models.CardItem.Services
{
    public interface ICardService
    {
        CardItem CreateCard(CardCreationInfo cardToCreate, Guid uId);
        Task<bool> AddCardAsync(CardItem cardToAddToRepo, CancellationToken cancellationToken);
        Task<IEnumerable<CardItem>> GetAllUserCards(Guid uid, CancellationToken cancellationToken);
        Task<CardItem> GetCardByIdAsync(Guid id, CancellationToken cancellationToken);
        void CheckOwnership(CardItem card, Guid userId);
    }
}