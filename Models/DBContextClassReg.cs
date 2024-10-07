using Microsoft.EntityFrameworkCore;

namespace WebApplication9Municipal_Billing_System.Models
{
    public class DBContextClassReg : DbContext
    {
        public DBContextClassReg(DbContextOptions<DBContextClassReg> options)
            : base(options)
        {
        }

        // Use non-nullable DbSet with suppression operator
        public DbSet<Reg> Regs { get; set; } = null!;
         public DbSet<Bill> bills {get;set;}
        public DbSet<Water> waters {get;set;}
        public DbSet<Tarriff> tarriffs {get;set;}
        public DbSet<Electricity> electricities {get;set;}
        
    }
}
