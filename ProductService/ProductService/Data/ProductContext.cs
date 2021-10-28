using ProductService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> opt) : base(opt)
        {

        }

        //dit mapt de model Product aan een database tabel Products
        public DbSet<Product> Products { get; set; }
        //en de EggTypes
        public DbSet<EggType> EggTypes { get; set; }

        //in principe is bovenstaande voldoende, EF regelt de relaties
        //Les Jackson neemt echter het zekere voor het onzekere en
        //declared hieronder de boel nog even expliciet.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<EggType>()
                .HasMany(p => p.Flocks)
                .WithOne(p => p.EggType!)
                .HasForeignKey(p => p.EggTypeId);

            modelBuilder
            .Entity<Product>()
            .HasOne(p => p.EggType)
            .WithMany(p => p.Flocks)
            .HasForeignKey(p => p.EggTypeId);
        }



    }
}
