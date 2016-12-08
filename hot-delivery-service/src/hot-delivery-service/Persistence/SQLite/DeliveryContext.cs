using hot_delivery_service.Helpers;
using hot_delivery_service.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hot_delivery_service.Persistence.SQLite
{
    //стандартный dbcontext ef core
    //конфигурирование посредством fluent-api
    public class DeliveryContext : DbContext
    {
        public DbSet<Delivery> Deliveries { get; set; }

        public DeliveryContext(DbContextOptions options)
            :base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("DataSource=deliveries.db");
        //    base.OnConfiguring(optionsBuilder);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Delivery>().HasKey(d => d.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
