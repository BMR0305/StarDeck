using Microsoft.EntityFrameworkCore;

namespace StarDeck_API.Models
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        public DbSet<Users> users => Set<Users>();
    }
}
