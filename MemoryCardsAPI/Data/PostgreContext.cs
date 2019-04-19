using Microsoft.EntityFrameworkCore;
using Models.CardItem;
using Models.CardsCollection;
using Models.Training;
using Models.User;

namespace MemoryCardsAPI.Data
{
    public class PostgreContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Host=localhost;" +
                "Port=5432;" +
                "Database=cards;" +
                "Username=postgres;" +
                "Password=12345");
        }

        public DbSet<CardItem> CardItems { get; set; }
        public DbSet<CardItemInfo> CardItemsInfos { get; set; }
        public DbSet<CardsCollection> CardsCollections { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserInfo> UsersInfos { get; set; }
    }
}