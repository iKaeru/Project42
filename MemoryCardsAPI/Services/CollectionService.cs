using System;
using System.Collections.Generic;
using System.Linq;
using Project42.Helpers;
using MemoryCardsAPI.Data;
using Model = Models.CardItem;
using View = Client.Models.CardItem;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.CardsCollection;

namespace Project42.Services
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

    public class CollectionService : ICollectionService
    {
        private DataContext _context;
        //private PostgreContext _context = new PostgreContext();
        public CollectionService(DataContext context)
        {
            _context = context;
        }

        public bool AddCard(string collectionName, Guid cardId, Guid userId)
        {
            var desiredCollection = _context.Collections
                .Where(x => x.UserId == userId)
                .FirstOrDefault(x => x.Name == collectionName);

            if (desiredCollection.CardItems.Contains(cardId))
            {
                return false;
            }

            desiredCollection.CardItems.Add(cardId);
            _context.Collections.Update(desiredCollection);
            _context.SaveChangesAsync();
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
            await _context.Collections.AddAsync(card);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public bool NameExists(string collectionName, Guid userId)
        {
            return _context.Collections
                                .Where(x => x.UserId == userId)
                                .Any(x => x.Name == collectionName);
        }

        public CardsCollection FindByName(string collectionName, Guid userId)
        {
            return _context.Collections
                .Where(x => x.UserId == userId)
                .FirstOrDefault(x => x.Name == collectionName);
        }

        public IEnumerable<CardsCollection> GetCollections(Guid userId)
        {
            return _context.Collections.Where(x => x.UserId == userId);
        }
    }
}