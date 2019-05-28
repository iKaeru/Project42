using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Data;
using Models.Errors;

namespace Models.CardItem.Repositories
{
    public class InMemoryCardsRepository : ICardsRepository
    {
        private InMemoryContext context;

        public InMemoryCardsRepository(InMemoryContext context)
        {
            this.context = context;
        }

        public async Task<CardItemInfo> CreateAsync(CardItem cardToCreate, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();
            var card = cardToCreate;
            card.Id = id;

            await context.Cards.AddAsync(card);
            await context.SaveChangesAsync();
            return card;
        }

        public Task<IEnumerable<CardItem>> GetAllUserCards(Guid uId, CancellationToken cancellationToken)
        {
            return Task.FromResult<IEnumerable<CardItem>>
                (context.Cards.Where(x => x.UserId == uId || x.UserId == default(Guid)));
        }

        public Task<IEnumerable<Guid>> GetAllUserCardsId(Guid uId, CancellationToken cancellationToken)
        {
            return Task.FromResult<IEnumerable<Guid>>(context.Cards
                .Where(x => x.UserId == uId || x.UserId == default(Guid))
                .Select(u => u.Id));
        }

        public Task<IReadOnlyList<CardItemInfo>> SearchAsync(CardSearchInfo query, CancellationToken cancelltionToken)
        {
            throw new NotImplementedException();
        }

        public async Task<CardItem> GetAsync(Guid cardId, CancellationToken cancellationToken)
        {
            return await context.Cards.FirstOrDefaultAsync(x => x.Id == cardId);
        }

        public async Task<CardItem> PatchAsync(CardItem patchInfo, CancellationToken cancelltionToken)
        {
            var card = context.Cards.Update(patchInfo);
            await context.SaveChangesAsync();
            return card.Entity;
        }

        public async Task<bool> DeleteCardAsync(Guid id)
        {
            var cardItem = await context.Cards.FindAsync(id);
            if (cardItem != null)
            {
                context.Cards.Remove(cardItem);
                context.SaveChanges();
                return true;
            }

            return false;
        }

        public async Task<bool> DeleteCardsFromListAsync(ICollection<Guid> cardsList)
        {
            foreach (var cardId in cardsList)
            {
                var cardItem = await context.Cards.FindAsync(cardId);
                if (cardItem != null)
                {
                    context.Cards.Remove(cardItem);
                    context.SaveChanges();
                    continue;
                }

                throw new AppException($"Card with id {cardId} not found");
            }

            return true;
        }

        public async Task<IEnumerable<CardItem>> GetCardsFromListAsync(IEnumerable<Guid> cardsList)
        {
            var result = new List<CardItem>();
            foreach (var cardId in cardsList)
            {
                var cardItem = await context.Cards.FirstOrDefaultAsync(x => x.Id == cardId);
                result.Add(cardItem);
            }

            return result;
        }
    }
}