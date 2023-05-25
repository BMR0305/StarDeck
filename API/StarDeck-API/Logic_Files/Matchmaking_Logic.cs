using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StarDeck_API.DB_Calls;
using StarDeck_API.Models;
using System.Data;
using System.Diagnostics;

namespace StarDeck_API.Logic_Files
{
    public class Matchmaking_Logic
    {
        private static Matchmaking_Logic instance = null;
        private Matchmaking_DB CallDB = Matchmaking_DB.GetInstance();

        public static Matchmaking_Logic GetInstance()
        {
            if (instance == null)
            {
                instance = new Matchmaking_Logic();
            }
            return instance;
        }

        public async Task<string> LookForGame(DBContext context, string email)
        {
            try
            {
                var PlayersWaiting = CallDB.GetUsersBP();

                if (PlayersWaiting.Count > 0)
                {
                    for (int i = 0; i < PlayersWaiting.Count; i++)
                    {
                        if (PlayersWaiting[i].u_status == "BP" && PlayersWaiting[i].email != email)
                        {
                            CallDB.UpdateUserStatus(PlayersWaiting[i].email, "EP");
                            Users current_user = CardsUsers_DB.GetInstance().GetUser(email)[0];
                            CallDB.UpdateUserStatus(email, "EP");

                            var planets = Planet_DB.GetInstance().GetGamePlanets();

                            Partida partida = CreateGameObject(current_user, PlayersWaiting[i], planets);

                            //Crear aux para enviar al front end la lista de los planetas y jugadores como objetos completos.
                            Partida_DTO partida_DTO = CreatePartida_DTO(partida, current_user, PlayersWaiting[i], planets);

                            string json_partida = JsonConvert.SerializeObject(partida_DTO);

                            return json_partida;
                        }
                    }
                }
                CallDB.UpdateUserStatus(email, "BP");
                string output = await WaitingGame(context, email);
                return output;

            }
            catch (System.Exception e)
            {
                return e.Message;
            }
        }

        public async Task<string> WaitingGame(DBContext context, string email)
        {

            Users user;
            CardsUsers_DB UsersDB_call = CardsUsers_DB.GetInstance();

            // Time limit to wait for a match to be found (in seconds)
            int timeout = 20;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            user = UsersDB_call.GetUser(email)[0];
            while (user.u_status == "BP" && stopwatch.Elapsed.TotalSeconds < timeout)
            {
                await Task.Delay(2000);
                user = UsersDB_call.GetUser(email)[0];
                if (user.u_status != "BP")
                {
                    //stopwatch.Stop();
                    break;
                }
            }

            var partidaList = context.partida.FromSqlRaw("EXEC GetUserMatch @email = {0}", email).ToList();

            if (partidaList.Count > 0)
            {
                Partida partida = partidaList[0];
                List<Planet> planets = new List<Planet>();
                planets.Add(Planet_DB.GetInstance().GetPlanetByID(partida.Planet1)[0]);
                planets.Add(Planet_DB.GetInstance().GetPlanetByID(partida.Planet2)[0]);
                planets.Add(Planet_DB.GetInstance().GetPlanetByID(partida.Planet3)[0]);
                Users opponent = UsersDB_call.GetUserByID(partida.Player1)[0];
                Partida_DTO partida_DTO = CreatePartida_DTO(partida, opponent, user, planets); 

                string json_partida = JsonConvert.SerializeObject(partida_DTO);
                return json_partida;
            }
            CallDB.UpdateUserStatus(email, "A");
            Message mess = new Message();
            mess.message = "Timeout reached";
            string output = JsonConvert.SerializeObject(mess);

            return output;

        }

        public Partida CreateGameObject(Users user, Users opponent, List<Planet> planets)
        {
            Partida partida = new Partida();
            partida.ID = KeyGen.GetInstance().CreatePattern("P-");
            partida.Player1 = user.ID;
            partida.Player2 = opponent.ID;
            partida.Planet1 = planets[0].ID;
            partida.Planet2 = planets[1].ID;
            partida.Planet3 = planets[2].ID;
            partida.p_status = "EC";
            CallDB.AddGame(partida);
            return partida;
        }

        public Partida_DTO CreatePartida_DTO(Partida partida, Users user, Users opponent, List<Planet> planets)
        {
            Partida_DTO partida_DTO = new Partida_DTO();
            partida_DTO.ID = partida.ID;
            partida_DTO.Players = new List<Users>();
            partida_DTO.Players.Add(opponent);
            partida_DTO.Players.Add(user);
            partida_DTO.Planets = planets;
            partida_DTO.p_status = partida.p_status;
            return partida_DTO;
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

        public Matchmaking_Logic()
        {
            
        }
    }
}
