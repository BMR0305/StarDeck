using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StarDeck_API.Models;
using System.Data;

namespace StarDeck_API.DB_Calls
{
    public class Matchmaking_DB
    {
        private static Matchmaking_DB instance = null;
        private DBContext context;

        public static Matchmaking_DB GetInstance()
        {
            if (instance == null)
            {
                instance = new Matchmaking_DB();
            }
            return instance;
        }

        public List<Users> GetUsersBP()
        {
            return context.users.FromSqlRaw("EXEC GetPlayersBP").ToList();
        }

        public void UpdateUserStatus(string email, string status)
        {
            context.Database.ExecuteSqlRaw("EXEC UpdateUserStatus @email = {0}, @status = {1}", email, status);
        }

        public void AddGame(Partida partida)
        {
            try
            {
                context.partida.Add(partida);
                context.SaveChanges();
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        public List<Partida> GetPlayerMatch(string email)
        {
            List<Partida> game =  context.partida.FromSqlRaw("EXEC GetUserMatch @email = {0}", email).ToList();
            if (game.Count == 0)
            {
                throw new Exception("Player has no game");
            }
            return game;
        }

        public void SetContext(DBContext context)
        {
            this.context = context;
        }
        private Matchmaking_DB() { }
    }
}
