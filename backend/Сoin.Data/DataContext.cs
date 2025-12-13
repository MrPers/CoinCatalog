using Base.Data;
using Microsoft.EntityFrameworkCore;
using Сoin.Entity.DB;

namespace Сoin.Data
{
    public class DataContext : BaseDataContext
    {
        public DbSet<CoinRate> CoinRates { get; set; }
        public DbSet<Coin> Coins { get; set; }
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

            modelBuilder.Entity<Coin>()
                .HasIndex(u => u.Name).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
