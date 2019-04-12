using Microsoft.EntityFrameworkCore;
using Models.CardItem;
using Models.CardsCollection;

namespace MemoryCardsAPI.Data
{
    public class CardItemsContext: DbContext
    {
        public CardItemsContext(DbContextOptions<CardItemsContext> options)
            : base(options)
        {
        }

        public DbSet<CardItem> CardItems { get; set; }
        public DbSet<CardsCollection> CardsCollections { get; set; }
    }
}
