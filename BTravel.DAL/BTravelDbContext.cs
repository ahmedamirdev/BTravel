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

        public DbSet<ContractRoom> ContractRooms { get; set; }

        public DbSet<ContractFile> ContractFiles { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<AppService> AppServices { get; set; }

        public DbSet<RoleAppService> RoleAppServices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region CommonUser

            modelBuilder.Entity<CommonUser>()
                //.ToTable("commonUser")
                .HasIndex(u => u.CommonUserId)
                .IsUnique();

            modelBuilder.Entity<CommonUser>()
              .HasOne(u => u.Role)
              .WithMany(c => c.CommonUsers)
              .HasForeignKey(f => f.RoleId);

            #endregion

            #region Contract

            modelBuilder.Entity<Entities.Contract>()
                .HasIndex(u => u.ContractId)
                .IsUnique();

            modelBuilder.Entity<Entities.Contract>()
                .HasOne(u => u.CommonUser)
                .WithMany(c => c.Contracts)
                .HasForeignKey(f => f.CommonUserId);

            #endregion

            #region ContractFile

            modelBuilder.Entity<ContractFile>()
            .HasIndex(i => i.ContractFileID)
            .IsUnique();

            modelBuilder.Entity<ContractFile>()
              .HasOne(u => u.Contract)
              .WithMany(c => c.Files)
              .HasForeignKey(f => f.ContractId);

            #endregion

            #region ContractRoom

            modelBuilder.Entity<ContractRoom>()
                .HasIndex(i => i.ContractRoomId)
                .IsUnique();

            modelBuilder.Entity<ContractRoom>()
               .HasOne(u => u.Contract)
               .WithMany(c => c.Rooms)
               .HasForeignKey(f => f.ContractId);

            #endregion

            #region Role

            modelBuilder.Entity<Role>()
                .HasIndex(u => u.RoleId)
                .IsUnique();

            #endregion

            #region AppService

            modelBuilder.Entity<AppService>()
                .HasIndex(u => u.AppServiceId)
                .IsUnique();

            #endregion

            #region RoleAppService

            modelBuilder.Entity<RoleAppService>()
                .HasIndex(u => u.RoleAppServiceId)
                .IsUnique();

            modelBuilder.Entity<RoleAppService>()
               .HasOne(u => u.Role)
               .WithMany(c => c.RoleAppServices)
               .HasForeignKey(f => f.RoleId);

            modelBuilder.Entity<RoleAppService>()
               .HasOne(u => u.AppService)
               .WithMany(c => c.RoleAppServices)
               .HasForeignKey(f => f.AppServiceId);

            #endregion
        }
    }
}