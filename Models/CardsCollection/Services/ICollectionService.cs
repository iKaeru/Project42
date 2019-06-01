using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models.CardsCollection.Services
{
    public interface ICollectionService
    {
        CardsCollection CreateCollection(Guid userId, string collectionName);
        Task<bool> AddCollectionAsync(CardsCollection collectionToAdd);
        Task<bool> AddCardToCollectionAsync(Guid collectionId, Guid cardId, Guid userId);
        Task<IEnumerable<CardsCollection>> GetAllCollectionsAsync(Guid userId);
        Task<CardsCollection> FindCollectionByNameAsync(string collectionName, Guid userId);
        Task<CardsCollection> FindCollectionByIdAsync(Guid collectionId, Guid userId);
        Task<bool> IsIdExistAsync(Guid collectionId, Guid userId);
        Task<bool> IsNameExistAsync(string collectionName, Guid userId);
        Task<IEnumerable<CardsCollection>> GetLearnedCollectionsAsync(Guid userId);
        Task<IEnumerable<CardItem.CardItem>> GetAllLearnedCardsAsync(Guid collectionId, Guid userId);
        Task<IEnumerable<CardItem.CardItem>> GetAllUnlearnedCardsAsync(Guid collectionId, Guid userId);
        void UpdateByIdAsync(CardsCollectionPatchInfo collection, Guid collectionId, Guid userId);
        Task<bool> Delete(Guid userId, Guid collectionId);
 }
}
