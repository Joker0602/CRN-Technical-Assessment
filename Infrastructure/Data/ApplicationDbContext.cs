using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product Configuration
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.ProductName)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(x => x.CreatedBy)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(x => x.CreatedOn)
                      .IsRequired();

                entity.Property(x => x.ModifiedBy)
                      .HasMaxLength(100);
            });

            // Item Configuration
            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Quantity)
                      .IsRequired();

                entity.HasOne(x => x.Product)
                      .WithMany(x => x.Items)
                      .HasForeignKey(x => x.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
