using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StarDeck_API.Models;
using System.Data;
using System.Diagnostics;

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

        public async Task<Partida> GetPlayerMatch(string email)
        {
            Debug.WriteLine("Hi im getting the player match");
            List<Partida> games = await context.partida.FromSqlRaw("EXEC GetUserMatch @email = {0}", email).ToListAsync();
            if (games.Count > 0)
            {
                return games[0];
            }
            else
            {
                return null;
            }
        }

        public void SetContext(DBContext context)
        {
            this.context = context;
        }
        private Matchmaking_DB() { }
    }
}
