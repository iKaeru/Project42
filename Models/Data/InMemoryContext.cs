using Microsoft.EntityFrameworkCore;

namespace Models.Data
{
    public class InMemoryContext : DbContext
    {
        public InMemoryContext(DbContextOptions<InMemoryContext> options) : base(options)
        {
        }

        public DbSet<User.User> Users { get; set; }
        public DbSet<CardItem.CardItem> Cards { get; set; }
        public DbSet<Models.CardsCollection.CardsCollection> Collections { get; set; }
        public DbSet<Models.Training.Training> Trainings { get; set; }
    }
}