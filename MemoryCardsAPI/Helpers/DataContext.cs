﻿using Models.CardItem;
using Microsoft.EntityFrameworkCore;
using Models.User;

namespace Project42.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<CardItem> Cards { get; set; }
    }
}