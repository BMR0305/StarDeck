using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace StarDeck_API.Models
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        public DbSet<Users> users => Set<Users>(); 

        public DbSet<Race> race => Set<Race>();

        public DbSet<User_Card> user_card => Set<User_Card>();

        public DbSet<Card> cards => Set<Card>();

        public DbSet<Planet> planet => Set<Planet>();

        //public DbSet<JoinUserCards> joinUserCards => Set<JoinUserCards>();

        public DbSet<Partida> partida => Set<Partida>();

        public DbSet<Deck> deck => Set<Deck>();

        public DbSet<Deck_Card> deck_card => Set<Deck_Card>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasKey(x => x.ID);

            modelBuilder.Entity<Race>().HasKey(x => x.r_name);

            modelBuilder.Entity<User_Card>().HasKey(x => new { x.user_key, x.card_key });

            modelBuilder.Entity<Card>().HasKey(x => x.ID);

            modelBuilder.Entity<Planet>().HasKey(x => x.ID);

            modelBuilder.Entity<Partida>().HasKey(x => x.ID);

            modelBuilder.Entity<Deck>().HasKey(x => x.Deck_ID);

            modelBuilder.Entity<Deck_Card>().HasKey(x => new { x.Deck_ID, x.Card_ID });
        }
    }
}
