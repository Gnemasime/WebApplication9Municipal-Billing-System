using Microsoft.EntityFrameworkCore;

namespace WebApplication9Municipal_Billing_System.Models
{
    public class DBContextClassReg : DbContext
    {
        public DBContextClassReg(DbContextOptions<DBContextClassReg> options)
            : base(options)
        {
        }

        // Define DbSet properties for each entity
        public DbSet<Reg> Regs { get; set; } = null!;
        public DbSet<Bill> bills { get; set; } = null!;
        public DbSet<Water> waters { get; set; } = null!;
        public DbSet<Tarriff> tarriffs { get; set; } = null!;
        public DbSet<Electricity> electricities { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Water>()
             .Property(w => w.status)
             .HasConversion<string>(); // Store as string in the database

            modelBuilder.Entity<Bill>()
                .HasOne(b => b.Reg)
                .WithMany()
                .HasForeignKey(b => b.RegUserId)
                .OnDelete(DeleteBehavior.NoAction); // No cascade delete for Reg (User)

           
            modelBuilder.Entity<Electricity>().Property(e => e.Cost).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Electricity>().Property(e => e.Rate).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Electricity>().Property(e => e.Usage).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Tarriff>().Property(t => t.DiscRate).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Water>().Property(w => w.Cost).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Water>().Property(w => w.Rate).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Water>().Property(w => w.Usage).HasColumnType("decimal(18,2)");
        }


    }
}