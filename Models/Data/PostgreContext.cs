using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Models.Data
{
    public class PostgreContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(  //Local Fedya DB
                "Host=localhost;" +
                "Port=5432;" +
                "Database=cards;" +
                "Username=postgres;" +
                "Password=1q2w3e");


            //optionsBuilder.UseNpgsql(  //Cloud DB
            //        "Host=rc1b-xfcvuyuupeyfsv8y.mdb.yandexcloud.net;" +
            //        "Port=6432;" +
            //        "Database=db1;" +
            //        "SSL Mode = Prefer;" +
            //        "Trust Server Certificate = true;" +
            //        "Username=user1;" +
            //        "Password=1q2w3e4r");

        }
        //    public PostgreContext(DbContextOptions<PostgreContext> options) : base(options) { }

        public DbSet<User.User> Users { get; set; }
        public DbSet<CardItem.CardItem> Cards { get; set; }
        public DbSet<Models.CardsCollection.CardsCollection> Collections { get; set; }
        public DbSet<Models.Training.Training> Trainings { get; set; }
        public DbSet<Models.Token.PasswordResetToken> PasswordResetTokens { get; set; }
    }
}