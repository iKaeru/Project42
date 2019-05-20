using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Models.Data;

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
            
            await context.AddAsync(card);
            await context.SaveChangesAsync();
            return card;
        }
        
        public Task<IEnumerable<CardItem>> GetAllUserCards(Guid uId, CancellationToken cancellationToken)
        {
            return Task.FromResult<IEnumerable<CardItem>>
                (context.Cards.Where(x => x.UserId == uId || x.UserId == default(Guid)));
        }

        public Task<IReadOnlyList<CardItemInfo>> SearchAsync(CardSearchInfo query, CancellationToken cancelltionToken)
        {
            throw new NotImplementedException();
        }

        public Task<CardItem> GetAsync(Guid cardId, CancellationToken cancellationToken)
        {
            return Task.FromResult
                (context.Cards.FirstOrDefault(x => x.Id == cardId));
        }

        public Task<CardItem> PatchAsync(CardPatchInfo patchInfo, CancellationToken cancelltionToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(Guid cardId, CancellationToken cancelltionToken)
        {
            throw new NotImplementedException();
        }
    }
}