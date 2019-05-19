using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Models.Data;

namespace Models.CardItem.Repositories
{
    public class PostgreCardsRepository : ICardsRepository
    {    
        private PostgreContext context = new PostgreContext();

        public async Task<CardItemInfo> CreateAsync(CardItem cardToCreate, CancellationToken cancellationToken)
        {
            await context.CardItems.AddAsync(cardToCreate);
            await context.SaveChangesAsync();
            return cardToCreate; // todo
        }

        public Task<IEnumerable<CardItem>> GetAllUserCards(Guid uId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<CardItemInfo>> SearchAsync(CardSearchInfo query, CancellationToken cancelltionToken)
        {
            throw new NotImplementedException();
        }

        public Task<CardItemInfo> GetAsync(Guid cardId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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