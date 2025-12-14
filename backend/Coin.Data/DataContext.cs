using Base.Data;
using Microsoft.EntityFrameworkCore;
using Coin.Entity.DB;

namespace Coin.Data
{
    public class DataContext : BaseDataContext
    {
        public DbSet<CoinRate> CoinRates { get; set; }
        public DbSet<Entity.DB.Coin> Coins { get; set; }
        public IQueryable<CoinRate> GetCoins(int id, int stepTime) => FromExpression(() => GetCoins(id, stepTime));

        public DataContext(DbContextOptions<DataContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDbFunction(() => GetCoins(default, default));

            modelBuilder.Entity<CoinRate>()
                .HasOne(sc => sc.Coin)
                .WithMany(s => s.CoinRate)
                .HasForeignKey(sc => sc.CoinId)
                .HasPrincipalKey(sc => sc.Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Entity.DB.Coin>()
                .HasIndex(u => u.Name).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
