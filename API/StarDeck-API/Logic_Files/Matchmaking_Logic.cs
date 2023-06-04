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
        private static object lockObject = new object();

        public static Matchmaking_Logic GetInstance()
        {
            if (instance == null)
            {
                instance = new Matchmaking_Logic();
            }
            return instance;
        }

        public async Task<string> LookForGame(string email)
        {
            try
            {
                var PlayersWaiting = CallDB.GetUsersBP();

                //lock (lockObject)

                if (PlayersWaiting.Count > 0)
                {
                    for (int i = 0; i < PlayersWaiting.Count; i++)
                    {
                        if (PlayersWaiting[i].u_status == "BP" && PlayersWaiting[i].email != email)
                        {
                            //CallDB.UpdateUserStatus(PlayersWaiting[i].email, "EP");
                            Users current_user = CardsUsers_DB.GetInstance().GetUser(email)[0];
                            CallDB.UpdateUserStatus(email, "EP");

                            var planets = Planet_DB.GetInstance().GetGamePlanets();

                            Partida partida = await Task.Run(()=> CreateGameObject(current_user, PlayersWaiting[i], planets));

                            //Crear aux para enviar al front end la lista de los planetas y jugadores como objetos completos.
                            Partida_DTO partida_DTO = CreatePartida_DTO(partida, current_user, PlayersWaiting[i], planets);

                            CallDB.UpdateUserStatus(PlayersWaiting[i].email, "EP");

                            Match_Logic.GetInstance.InitialTurn(partida.ID, email);

                            string json_partida = JsonConvert.SerializeObject(partida_DTO);

                            return json_partida;
                        }
                    }
                }
                CallDB.UpdateUserStatus(email, "BP");
                string output = await WaitingGame(email);
                return output;

            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /**
         * Method that keeps a player searching for a match until a match is found or a timeout is reached.
         * Params: email - email of the player that is searching for a match.
         * Return: A string with the information of the match found or a message if a timeout is reached.
         */
        public async Task<string> WaitingGame(string email)
        {
            CardsUsers_DB UsersDB_call = CardsUsers_DB.GetInstance();
            Partida game = null;

            // Time limit to wait for a match to be found (in seconds)
            int timeout = 20;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Users user = UsersDB_call.GetUser(email)[0];

            while (stopwatch.Elapsed.TotalSeconds < timeout)
            {
                game = await Task.Run(() => CallDB.GetPlayerMatch(email));
                if (game != null)
                {
                    stopwatch.Stop();
                    Debug.WriteLine("Entro al IF");
                    List<Planet> planets = new List<Planet>();
                    await Task.Run(() => planets.Add(Planet_DB.GetInstance().GetPlanetByID(game.Planet1)[0]));
                    await Task.Run(() => planets.Add(Planet_DB.GetInstance().GetPlanetByID(game.Planet2)[0]));
                    await Task.Run(() => planets.Add(Planet_DB.GetInstance().GetPlanetByID(game.Planet3)[0]));
                    Debug.WriteLine("Obtuvo los planetas");
                    Users opponent = await Task.Run(() => UsersDB_call.GetUserByID(game.Player1)[0]);
                    Debug.WriteLine("Oponente");
                    Partida_DTO partida_DTO = CreatePartida_DTO(game, opponent, user, planets);
                    Debug.WriteLine("Obtuvo la partida");

                    string json_partida = JsonConvert.SerializeObject(partida_DTO);
                    return json_partida;

                }
                Debug.WriteLine("Esto es antes del delay");
                await Task.Delay(2000);
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
            partida.Winner = "P-NULL";
            partida.C_Turn = "C-NULL";
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
