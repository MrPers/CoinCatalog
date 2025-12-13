using Microsoft.EntityFrameworkCore;

namespace Base.Data
{
    public class BaseDataContext : DbContext
    {
        public BaseDataContext(DbContextOptions options) : base(options)
        {
        }
    }
}