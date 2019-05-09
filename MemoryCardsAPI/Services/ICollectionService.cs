using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.CardsCollection;

namespace MemoryCardsAPI.Services
{
    public interface ICollectionService
    {
        IEnumerable<CardsCollection> GetCollections(Guid userId);
        bool AddCard(string collectionName, Guid cardId, Guid userId);
        bool DeleteCard(Guid CardId);
        Task AddAsync(CardsCollection cardCollection);
        Task SaveChangesAsync();
        bool NameExists(string v, Guid userId);
        CardsCollection FindByName(string collectionName, Guid userId);
    }
}