using System;
using System.Collections.Generic;
using MemoryCardsAPI.Helpers;
using Model = Models.CardItem;
using View = Client.Models.CardItem;
using System.Threading.Tasks;
using MemoryCardsAPI.Data;

namespace MemoryCardsAPI.Services
{
    public class CardService : ICardService
    {
        private DataContext context;
        //private PostgreContext _context = new PostgreContext();
        public CardService(DataContext context)
        {
            this.context = context;
        }

        public IEnumerable<Model.CardItem> GetAll()
        {
            return context.Cards;
        }

        public Model.CardItem GetById(Guid id)
        {
            return context.Cards.Find(id);
        }

        public Model.CardItem Create(Model.CardItem card)
        {
            context.Cards.Add(card);
            context.SaveChanges();

            return card;
        }

        public void Update(Model.CardItem card)
        {
            var oldCard = context.Cards.Find(card.Id);

            if (oldCard == null)
                throw new AppException("Card not found");

            context.Cards.Update(card);
            context.SaveChanges();
        }

        public void Delete(int id)
        {
            var card = context.Cards.Find(id);
            if (card != null)
            {
                context.Cards.Remove(card);
                context.SaveChanges();
            }
        }

        public async Task AddAsync(Model.CardItem card)
        {
           await context.Cards.AddAsync(card);
        }

        public async Task SaveChangesAsync()
        {
           await context.SaveChangesAsync();
        }
    }
}