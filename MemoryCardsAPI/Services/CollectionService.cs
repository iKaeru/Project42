using System;
using System.Collections.Generic;
using System.Linq;
using MemoryCardsAPI.Data;
using Model = Models.CardItem;
using View = Client.Models.CardItem;
using System.Threading.Tasks;
using Models.CardsCollection;

namespace MemoryCardsAPI.Services
{
    public class CollectionService : ICollectionService
    {
        private DataContext context;
        //private PostgreContext _context = new PostgreContext();
        public CollectionService(DataContext context)
        {
            this.context = context;
        }

        public bool AddCard(string collectionName, Guid cardId, Guid userId)
        {
            var desiredCollection = context.Collections
                .Where(x => x.UserId == userId)
                .FirstOrDefault(x => x.Name == collectionName);

            if (desiredCollection.CardItems.Contains(cardId))
            {
                return false;
            }

            desiredCollection.CardItems.Add(cardId);
            context.Collections.Update(desiredCollection);
            context.SaveChangesAsync();
            return true;
        }

        public bool DeleteCard(Guid CardId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Model.CardItem> GetCards()
        {
            throw new NotImplementedException();
        }


        public async Task AddAsync(CardsCollection card)
        {
            await context.Collections.AddAsync(card);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public bool NameExists(string collectionName, Guid userId)
        {
            return context.Collections
                                .Where(x => x.UserId == userId)
                                .Any(x => x.Name == collectionName);
        }

        public CardsCollection FindByName(string collectionName, Guid userId)
        {
            return context.Collections
                .Where(x => x.UserId == userId)
                .FirstOrDefault(x => x.Name == collectionName);
        }

        public IEnumerable<CardsCollection> GetCollections(Guid userId)
        {
            return context.Collections.Where(x => x.UserId == userId);
        }
    }
}