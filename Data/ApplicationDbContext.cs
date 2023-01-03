using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;
using Zdm_management.Models;
namespace Zdm_management.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<IpWishList> IpWishLists { get; set; }
        public DbSet<Leave> leaves { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder
            .Entity<ApplicationUser>()
            .HasMany(apu => apu.Attendances);


            modelBuilder.Entity<ApplicationUser>()
             .Navigation(apu => apu.Attendances)
             .UsePropertyAccessMode(PropertyAccessMode.Property);

            base.OnModelCreating(modelBuilder);
            modelBuilder
            .Entity<ApplicationUser>()
            .HasMany(apu => apu.Attendances);

            modelBuilder.Entity<ApplicationUser>()
             .Navigation(apu => apu.Leaves)
             .UsePropertyAccessMode(PropertyAccessMode.Property);
        }
    }
}