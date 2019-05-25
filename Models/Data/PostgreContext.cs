using Microsoft.EntityFrameworkCore;
using Models.CardItem;
using Models.User;

namespace Models.Data
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

        
        public DbSet<User.User> Users { get; set; }
        public DbSet<CardItem.CardItem> Cards { get; set; }
        public DbSet<Models.CardsCollection.CardsCollection> Collections { get; set; }
        public DbSet<Models.Training.Training> Trainings { get; set; }
        
//        public DbSet<CardItemInfo> CardItemsInfos { get; set; }
//        public DbSet<UserInfo> UsersInfos { get; set; }
    }
}