using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Models.CardItem.Repositories
{
    public class PostgreCardsRepository : ICardsRepository
    {    
        public Task<CardItemInfo> CreateAsync(CardCreationInfo creationInfo, CancellationToken cancellationToken)
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