using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models.CardsCollection.Repositories
{
    public interface ICollectionsRepository
    {
        Task<CardsCollection> CreateAsync(CardsCollection collectionToAdd);
        Task UpdateAsync(CardsCollection collection);
        Task<bool> FindNameAsync(string collectionName, Guid uId);
        Task<CardsCollection> FindByNameAsync(string collectionName, Guid userId);
        Task<IEnumerable<CardsCollection>> FindCollections(Guid userId);
        Task<CardsCollection> PatchAsync(CardsCollection patchInfo);
        Task<bool> DeleteCollectionAsync(Guid userId, string collectionName);
    }
}