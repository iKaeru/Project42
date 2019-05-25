using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models.CardsCollection.Services
{
    public interface ICollectionService
    {
        CardsCollection CreateCollection(Guid userId, string collectionName);
        Task<bool> AddCollectionAsync(CardsCollection collectionToAdd);
        Task<bool> AddCardToCollectionAsync(string collectionName, Guid cardId, Guid userId);
        Task<IEnumerable<CardsCollection>> GetAllCollectionsAsync(Guid userId);
        Task<CardsCollection> FindCollectionByNameAsync(string collectionName, Guid userId);
        Task<bool> IsNameExistAsync(string collectionName, Guid userId);
        Task<IEnumerable<CardsCollection>> GetLearnedCollectionsAsync(Guid uId);
    }
}