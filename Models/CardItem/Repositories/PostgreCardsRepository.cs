using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Data;

namespace Models.CardItem.Repositories
{
    public class PostgreCardsRepository : ICardsRepository
    {    
        private PostgreContext context;
        public PostgreCardsRepository(PostgreContext context)
        {
            this.context = context;
        }

        public async Task<CardItemInfo> CreateAsync(CardItem cardToCreate, CancellationToken cancellationToken)
        {
            await context.Cards.AddAsync(cardToCreate);
            await context.SaveChangesAsync();
            return cardToCreate;
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

        public async Task<CardItem> GetAsync(Guid cardId, CancellationToken cancellationToken)
        {
            return await context.Cards.FirstOrDefaultAsync(x => x.Id == cardId);
        }

        public Task<CardItem> PatchAsync(CardItem patchInfo, CancellationToken cancelltionToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(Guid cardId, CancellationToken cancelltionToken)
        {
            throw new NotImplementedException();
        }
    }
}