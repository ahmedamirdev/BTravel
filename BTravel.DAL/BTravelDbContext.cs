using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BTravel.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BTravel.DAL
{
    public class BTravelDbContext : DbContext
    {
        public BTravelDbContext()
        {
        }

        public BTravelDbContext(DbContextOptions<BTravelDbContext> options) : base(options)
        {
        }

        public DbSet<Entities.Contract> Contracts { get; set; }

        public DbSet<CommonUser> CommonUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        
            modelBuilder.Entity<Entities.Contract>()
                //.ToTable("commonUser")
                .HasIndex(u => u.ContractId)
                .IsUnique();
            modelBuilder.Entity<CommonUser>()
                //.ToTable("commonUser")
                .HasIndex(u => u.CommonUserId)
                .IsUnique();

            modelBuilder.Entity<Entities.Contract>()
                .HasOne(u => u.CommonUser)
                .WithMany(c => c.Contracts)
                .HasForeignKey(f => f.CommonUserId);
        }
    }

    
}