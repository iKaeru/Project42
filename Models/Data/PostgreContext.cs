using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models.CardItem;
using Models.User;

namespace Models.Data
{
    public class PostgreContext : DbContext
    {
        public PostgreContext(DbContextOptions<PostgreContext> options) : base(options) { }

        public DbSet<User.User> Users { get; set; }
        public DbSet<CardItem.CardItem> Cards { get; set; }
        public DbSet<Models.CardsCollection.CardsCollection> Collections { get; set; }
        public DbSet<Models.Training.Training> Trainings { get; set; }
    }
}