using Base.Data;
using Microsoft.EntityFrameworkCore;
using Coin.Entity.DB;

namespace Coin.Data
{
    public class DataContext : BaseDataContext
    {
        public DbSet<CoinPrice> CoinRates { get; set; }
        public DbSet<Entity.DB.CoinDetails> Coins { get; set; }
        public IQueryable<CoinPrice> GetCoins(int id, int stepTime) => FromExpression(() => GetCoins(id, stepTime));

        public DataContext(DbContextOptions<DataContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDbFunction(() => GetCoins(default, default));

            modelBuilder.Entity<CoinPrice>()
                .HasOne(sc => sc.Coin)
                .WithMany(s => s.CoinPrices)
                .HasForeignKey(sc => sc.CoinId)
                .HasPrincipalKey(sc => sc.Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Entity.DB.CoinDetails>()
                .HasIndex(u => u.Name).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
