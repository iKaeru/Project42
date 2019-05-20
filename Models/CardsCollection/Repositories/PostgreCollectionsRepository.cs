using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models.CardsCollection.Repositories
{
    public class PostgreCollectionsRepository : ICollectionsRepository
    {
        public Task<bool> FindNameAsync(string login)
        {
            throw new System.NotImplementedException();
        }

        public Task<CardsCollection> CreateAsync(CardsCollection collectionToAdd)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(CardsCollection collection)
        {
            throw new NotImplementedException();
        }

        public Task<bool> FindNameAsync(string collectionName, Guid uId)
        {
            throw new NotImplementedException();
        }

        public Task<CardsCollection> FindByNameAsync(string collectionName, Guid userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CardsCollection> FindCollections(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}