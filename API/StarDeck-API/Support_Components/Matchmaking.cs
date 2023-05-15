using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StarDeck_API.Models;
using System.Data;
using System.Diagnostics;

namespace StarDeck_API.Support_Components
{
    public class Matchmaking
    {
        private static Matchmaking instance = null;

        public static Matchmaking GetInstance()
        {
            if (instance == null)
            {
                instance = new Matchmaking();
            }
            return instance;
        }
        
        public async Task<string> LookForGame(DBContext context, string email)
        {
            try
            {
                var PlayersWaiting = context.users.FromSqlRaw("EXEC GetPlayersBP").ToList();

                if (PlayersWaiting.Count > 0)
                {
                    for (int i = 0; i < PlayersWaiting.Count; i++)
                    {
                        if (PlayersWaiting[i].u_status == "BP" && PlayersWaiting[i].email != email)
                        {
                            Users current_user = context.users.FromSqlRaw("EXEC GetPlayer @email = {0}", email).ToList()[0];
                            await context.Database.ExecuteSqlRawAsync("EXEC UpdateUserStatus @email = {0}, @status = {1}",email, "EP");
                            await context.Database.ExecuteSqlRawAsync("EXEC UpdateUserStatus @email = {0}, @status = {1}", PlayersWaiting[i].email, "EP");
                            var planets = await context.planet.FromSqlRaw("EXEC GetGamePlanets").ToListAsync();

                            Partida partida = new Partida();
                            partida.ID = KeyGen.GetInstance().CreatePattern("P-");
                            partida.Player1 = current_user.ID;
                            partida.Player2 = PlayersWaiting[i].ID;
                            partida.Planet1 = planets[0].ID;
                            partida.Planet2 = planets[1].ID;
                            partida.Planet3 = planets[2].ID;
                            partida.p_status = "EC";
                            context.partida.Add(partida);
                            await context.SaveChangesAsync();

                            //Crear aux para enviar al front end la lista de los planetas y jugadores como modelo en si?

                            string json_partida = JsonConvert.SerializeObject(partida);

                            return json_partida;
                        }
                    }
                }
                context.Database.ExecuteSqlRaw("EXEC UpdateUserStatus @email = {0}, @status = {1}", email, "BP");
                string output = await WaitingGame(context, email);
                return output;

            }
            catch (System.Exception e)
            {
                return e.Message;
            }
        }

        public async Task<string> WaitingGame(DBContext context,string email)
        {

            Users user = context.users.FromSqlRaw("EXEC GetPlayer @Email = {0}", email).ToList()[0];

            // Time limit to wait for a match to be found (in seconds)
            int timeout = 20;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (user.u_status == "BP" && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                await Task.Delay(500);
                user = context.users.FromSqlRaw("EXEC GetPlayer @Email = {0}", email).ToList()[0];
            }

            if (user.u_status == "EP")
            {
                var partida = context.partida.FromSqlRaw("EXEC GetUserMatch @Email = {0}", email).ToList()[0];
                string json_partida = JsonConvert.SerializeObject(partida);
                return json_partida;
            }
            else
            {
                context.Database.ExecuteSqlRaw("EXEC UpdateUserStatus @email = {0}, @status = {1}", email, "A");
                return "Timeout reached";
            }

        }

        public string CancelMM(DBContext context, string email)
        {
            try
            {
                context.Database.ExecuteSqlRaw("EXEC UpdateUserStatus @email = {0}, @status = {1}", email, "A");
                return "Matchmaking canceled";
            }
            catch (System.Exception e)
            {
                return e.Message;
            }
        }

        public Matchmaking()
        {
            
        }
    }
}
