using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Models.CardItem.Services
{
    public interface ICardService
    {
        CardItem CreateCard(CardCreationInfo cardToCreate, Guid userId);
        Task AddCardAsync(CardItem cardToAddToRepo, CancellationToken cancellationToken);
        Task<IEnumerable<CardItem>> GetAllUserCards(Guid userId, CancellationToken cancellationToken);
        Task<IEnumerable<Guid>> GetAllUserCardsId(Guid userId, CancellationToken cancellationToken);
        Task<CardItem> GetCardByIdAsync(Guid id, CancellationToken cancellationToken);
        void CheckOwnership(CardItem card, Guid userId);
        void UpdateCardByIdAsync(CardPatchInfo cardToUpdate, CancellationToken cancellationToken);
        Task<bool> Delete(Guid id);
        Task<IEnumerable<CardItem>> GetAllCardsFromList(IEnumerable<Guid> cardsList);
        Task<bool> DeleteCardsFromList(ICollection<Guid> cardsList);
    }
}