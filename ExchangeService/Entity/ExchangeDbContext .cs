using ExchangeService.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeService.Entity
{
    public class ExchangeDbContext : DbContext
    {
        public ExchangeDbContext(DbContextOptions options) : base(options) { }

        public DbSet<CurrencyExchangeTrade> Trades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<CurrencyExchangeTrade>().Property(x => x.Rate).HasPrecision(12, 10);
            modelBuilder.Entity<CurrencyExchangeTrade>().Property(x => x.TargetAmount).HasPrecision(12, 10);
            modelBuilder.Entity<CurrencyExchangeTrade>().Property(x => x.BaseAmount).HasPrecision(12, 10);

            base.OnModelCreating(modelBuilder);
        }
    }
}
