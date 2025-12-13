using Base.Data;
using Letter.Entity.DB;
using Microsoft.EntityFrameworkCore;

namespace Letter.Data
{
    public class DataContext : BaseDataContext
    {
        public DbSet<LetterEntity> Letters { get; set; }

        public DataContext(DbContextOptions<DataContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }


}
