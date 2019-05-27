using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Data;

namespace Models.CardsCollection.Repositories
{
    public class PostgreCollectionsRepository : ICollectionsRepository
    {
        private PostgreContext context;

        public PostgreCollectionsRepository(PostgreContext context)
        {
            this.context = context;
        }

        public async Task<bool> FindNameAsync(string collectionName, Guid uId)
        {
            return await context.Collections
                .Where(x => x.UserId == uId)
                .AnyAsync(x => x.Name == collectionName);
        }

        public async Task<bool> FindIdAsync(Guid collectionId, Guid uId)
        {
            return await context.Collections
                .Where(x => x.UserId == uId)
                .AnyAsync(x => x.Id == collectionId);
        }

        public async Task<CardsCollection> CreateAsync(CardsCollection collectionToAdd)
        {
            await context.Collections.AddAsync(collectionToAdd);
            await context.SaveChangesAsync();
            return collectionToAdd;
        }

        public async Task<CardsCollection> FindByNameAsync(string collectionName, Guid userId)
        {
            return await context.Collections
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync(x => x.Name == collectionName);
        }

        public async Task<CardsCollection> FindByIdAsync(Guid collectionId, Guid userId)
        {
            return await context.Collections
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync(x => x.Id == collectionId);
        }

        public async Task UpdateAsync(CardsCollection collection)
        {
            context.Collections.Update(collection);
            await context.SaveChangesAsync();
        }

        public Task<IEnumerable<CardsCollection>> FindCollections(Guid userId)
        {
            return Task.FromResult<IEnumerable<CardsCollection>>
                (context.Collections.Where(x => x.UserId == userId));
        }

        public async Task<CardsCollection> PatchAsync(CardsCollection patchInfo)
        {
            var card = context.Collections.Update(patchInfo);
            await context.SaveChangesAsync();
            return card.Entity;
        }

        public async Task<bool> DeleteCollectionAsync(Guid userId, Guid collectionId)
        {
            var collection = await context.Collections
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync(x => x.Id == collectionId);
            
            if (collection != null)
            {
                context.Collections.Remove(collection);
                context.SaveChanges();
                return true;
            }

            return false;
        }
    }
}