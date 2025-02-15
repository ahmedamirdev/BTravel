using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public DbSet<TestEntity> TestEntity { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestEntity>()
                //.ToTable("commonUser")
                .HasIndex(u => u.Id)
                .IsUnique();
        }
    }

    public class TestEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}