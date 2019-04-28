using System;
using System.Collections.Generic;
using System.Linq;
using Project42.Helpers;
using MemoryCardsAPI.Data;
using Model = Models.CardItem;
using View = Client.Models.CardItem;

namespace Project42.Services
{
    public interface ICardService
    {
        IEnumerable<Model.CardItem> GetAll();
        Model.CardItem GetById(Guid id);
        Model.CardItem Create(Model.CardItem card);
        void Update(Model.CardItem card);
        void Delete(int id);
        void AddAsync(Model.CardItem card);
        void SaveChangesAsync();
    }

    public class CardService : ICardService
    {
        private DataContext _context;
        //private PostgreContext _context = new PostgreContext();
        public CardService(DataContext context)
        {
            _context = context;
        }

        public IEnumerable<Model.CardItem> GetAll()
        {
            return _context.Cards;
        }

        public Model.CardItem GetById(Guid id)
        {
            return _context.Cards.Find(id);
        }

        public Model.CardItem Create(Model.CardItem card)
        {
            _context.Cards.Add(card);
            _context.SaveChanges();

            return card;
        }

        public void Update(Model.CardItem card)
        {
            var oldCard = _context.Cards.Find(card.Id);

            if (oldCard == null)
                throw new AppException("Card not found");

            _context.Cards.Update(card);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var card = _context.Cards.Find(id);
            if (card != null)
            {
                _context.Cards.Remove(card);
                _context.SaveChanges();
            }
        }

        public void AddAsync(Model.CardItem card)
        {
            _context.Cards.AddAsync(card);
        }

        public void SaveChangesAsync()
        {
            _context.SaveChangesAsync();
        }
    }
}