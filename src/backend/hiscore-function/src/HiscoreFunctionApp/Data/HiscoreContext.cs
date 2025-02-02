using HiscoreFunctionApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiscoreFunctionApp.Data
{
    public class HiscoreContext : DbContext
    {
        public HiscoreContext(DbContextOptions<HiscoreContext> options) : base(options)
        {
            Stats = Set<Stats>();
            Users = Set<Users>();
        }

        public DbSet<Stats> Stats { get; set; } = null!;
        public DbSet<Users> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserID);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Stats>(entity =>
            {
                entity.HasKey(e => e.StatID);
                entity.HasOne<Users>().WithMany().HasForeignKey(e => e.UserID);
            });
        }
    }
}
