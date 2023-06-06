using StarDeck_API.DB_Calls;
using StarDeck_API.Models;
using StarDeck_API.Logic_Files;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace StarDeck_API.Logic_Files
{
    public class Match_Logic
    {
        private static Match_Logic instance = null;
        private Match_DB CallDB = Match_DB.GetInstance;
        private KeyGen KeyGenerator = KeyGen.GetInstance();
        private static object lockObject = new object();

        public static Match_Logic GetInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Match_Logic();
                }
                return instance;
            }
        }

        public Turn InitialTurn(string gameID, string email)
        {
            Users user = CardsUsers_DB.GetInstance().GetUser(email)[0];
            Partida game = CallDB.GetGameByID(gameID);

            Turn turn = new Turn();

            turn.Turn_ID = CreateTurnID();
            turn.Players_Ready = 0;
            turn.Game_ID = gameID;
            turn.Turn_Number = 1;
            CallDB.InsertTurn(turn);
            CallDB.UpdateGameTurn(gameID, turn.Turn_ID);
            return turn;

        }

        public string CreateTurnID()
        {
            string id = "";
            List<Turn> turns = CallDB.GetTurns();
            if (turns.Count == 0)
            {
                id = KeyGenerator.CreatePattern("T-");
            }
            else
            {
                id = KeyGenerator.CreatePattern("T-");
                for (int i = 0; i < turns.Count; i++)
                {

                    if (turns[i].Turn_ID == id)
                    {
                        id = KeyGenerator.CreatePattern("T-");
                    }
                }
            }
            return id;
        }

        public void CreateNewTurn(Turn turn)
        {
            CallDB.InsertTurn(turn);
        }

        public async Task<string> EndTurn(List<CardPlayed> cardsPlayed, string gameID, string email)
        {
            Partida game = CallDB.GetGameByID(gameID);
            Users user = CardsUsers_DB.GetInstance().GetUser(email)[0];
            int MaxTurns = 8;

            if (game.TurnCount <= MaxTurns)
            {
                Turn turn = CallDB.GetTurnByID(game.C_Turn);
                lock (lockObject)
                {
                    InsertCardsPlayed(cardsPlayed);
                    if (turn.Players_Ready >= 1)
                    {
                        CallDB.AddPlayerReady(game.C_Turn);
                        Turn newTurn = new Turn();
                        newTurn.Turn_ID = CreateTurnID();
                        newTurn.Players_Ready = 0;
                        newTurn.Game_ID = gameID;
                        newTurn.Turn_Number = turn.Turn_Number+1;
                        CallDB.InsertTurn(newTurn);
                        CallDB.UpdateGameTurn(gameID, newTurn.Turn_ID);
                        CallDB.CountTurn(gameID);
                        return "Turno terminado";
                    }
                }
                CallDB.AddPlayerReady(game.C_Turn);
                return await WaitingEndTurn(game.ID,game.C_Turn);
            }

            return "Turno Maximo";
        }

        public async Task<string> WaitingEndTurn(string gameID,string turnID)
        {
            Partida game = CallDB.GetGameByID(gameID);
            bool ready = false;
            while (!ready)
            {
                if (game.C_Turn != turnID)
                {
                    return "Turno terminado";
                }
                await Task.Run(() => CallDB.RefreshGameCache(game));
                game = await Task.Run(()=> CallDB.GetGameByID(gameID));
                Debug.WriteLine(game.C_Turn);
                await Task.Delay(1500);
            }
            return "Turno terminado";
        }

        public void InsertCardsPlayed(List<CardPlayed> cardsPlayed)
        {
            for (int i = 0; i < cardsPlayed.Count; i++)
            {
                CallDB.InsertCardPlayed(cardsPlayed[i]);
            }
        }

        public void InsertDeckToCardsLeft(string gameID)
        {
            Partida game = CallDB.GetGameByID(gameID);

            Users player1 = CardsUsers_DB.GetInstance().GetUserByID(game.Player1)[0];
            Users player2 = CardsUsers_DB.GetInstance().GetUserByID(game.Player2)[0];

            CallDB.InsertDeckToCardsLeft(player1.ID, player1.current_deck);
            CallDB.InsertDeckToCardsLeft(player2.ID, player2.current_deck);
        }

        public string GetHand(string gameID, string email)
        {
            Partida game = CallDB.GetGameByID(gameID);
            Users user = CardsUsers_DB.GetInstance().GetUser(email)[0];
            int HandSize = 5;

            CallDB.InsertDeckToCardsLeft(user.ID, user.current_deck);
            List<CardsLeft> hand = CallDB.TakeCards(user.ID,HandSize);
            
            for (int i = 0; i < hand.Count; i++)
            {
                CallDB.DeleteCardLeft(user.ID, hand[i].Card_ID);
            }

            PlayerTurn_Card playerTurn_Card = new PlayerTurn_Card();
            playerTurn_Card.PlayerID = user.ID;
            playerTurn_Card.TurnID = game.C_Turn;
            playerTurn_Card.Card1_ID = hand[0].Card_ID;
            playerTurn_Card.Card2_ID = hand[1].Card_ID;
            playerTurn_Card.Card3_ID = hand[2].Card_ID;
            playerTurn_Card.Card4_ID = hand[3].Card_ID;
            playerTurn_Card.Card5_ID = hand[4].Card_ID;
            CallDB.InsertPlayerTurnCard(playerTurn_Card);

            string output = JsonConvert.SerializeObject(playerTurn_Card, Formatting.Indented);
            return output;
        }

        public string TakeCard(string email)
        {
            Users user = CardsUsers_DB.GetInstance().GetUser(email)[0];

            CardsLeft cardtaken = CallDB.TakeCards(user.ID, 1)[0];
            CallDB.DeleteCardLeft(user.ID, cardtaken.Card_ID);
            Card card = CardsUsers_DB.GetInstance().GetCard(cardtaken.Card_ID);

            string output = JsonConvert.SerializeObject(card, Formatting.Indented);

            return output;
        }

        public string GetCardsPlayed(string gameID, string turnID, string email)
        {
            lock (lockObject)
            {
                Users player = CardsUsers_DB.GetInstance().GetUser(email)[0];
                Partida game = CallDB.GetGameByID(gameID);
                string otherp = "";
                if (player.ID == game.Player1)
                {
                    otherp = game.Player2;
                }
                else
                {
                    otherp = game.Player1;
                }

                List<CardPlayed> cardsPlayed = CallDB.GetCardsPlayed(gameID, turnID, otherp);

                List<CardPlayed_DTO> cardsPlayed_DTO = new List<CardPlayed_DTO>();

                for (int i = 0; i < cardsPlayed.Count; i++)
                {
                    CardPlayed_DTO cardPlayed_DTO = new CardPlayed_DTO();
                    cardPlayed_DTO.GameID = cardsPlayed[i].GameID;
                    cardPlayed_DTO.Card = CardsUsers_DB.GetInstance().GetCard(cardsPlayed[i].CardID);
                    cardPlayed_DTO.PlayerID = cardsPlayed[i].PlayerID;
                    cardPlayed_DTO.Turn = cardsPlayed[i].Turn;
                    cardPlayed_DTO.Planet = cardsPlayed[i].Planet;
                    cardsPlayed_DTO.Add(cardPlayed_DTO);
                }

                string output = JsonConvert.SerializeObject(cardsPlayed_DTO, Formatting.Indented);
                return output;
            }
        }

        public string EndGame(string gameID)
        {
            Partida game = CallDB.GetGameByID(gameID);
            lock (lockObject)
            {
                if (game.Winner == "P-NULL")
                {
                    List<CardPlayed> cardsPlayed = CallDB.GetCardsPlayedFullGame(gameID, game.Player1);
                    List<CardPlayed> cardsPlayed2 = CallDB.GetCardsPlayedFullGame(gameID, game.Player2);

                    string cards1 = "";
                    string cards2 = "";

                    for (int i = 0; i < cardsPlayed.Count; i++)
                    {
                        cards1 += cardsPlayed[i].CardID + "#";
                    }

                    for (int i = 0; i < cardsPlayed2.Count; i++)
                    {
                        cards2 += cardsPlayed2[i].CardID + "#";
                    }

                    int score1 = CallDB.GetUserPoints(cards1);
                    int score2 = CallDB.GetUserPoints(cards1);

                    string output = "";
                    if (score1 > score2)
                    {
                        CallDB.UpdateWinner(game.Player1, gameID);
                        Users player = CardsUsers_DB.GetInstance().GetUserByID(game.Player1)[0];
                        output = JsonConvert.SerializeObject(player, Formatting.Indented);
                    }
                    else if (score1 == score2)
                    {
                        CallDB.UpdateWinner("Tie", gameID);
                        output = "";
                    }
                    else
                    {
                        CallDB.UpdateWinner(game.Player2, gameID);
                        Users player = CardsUsers_DB.GetInstance().GetUserByID(game.Player2)[0];
                        output = JsonConvert.SerializeObject(player, Formatting.Indented);
                    }
                    return output;
                }
                else
                {
                    Users user = CardsUsers_DB.GetInstance().GetUserByID(game.Winner);
                    string output = JsonConvert.SerializeObject(user, Formatting.Indented);
                    return output;
                }
            }
        }

        public string GetGameTurn(string gameID)
        {
            Partida game = CallDB.GetGameByID(gameID);
            Turn turn = CallDB.GetTurnByID(game.C_Turn);
            Turn_DTO turn_DTO = new Turn_DTO();
            turn_DTO.TurnID = turn.Turn_ID;
            turn_DTO.TurnNumber = turn.Turn_Number;
            turn_DTO.Energy = (2 * turn.Turn_Number) - 1;
            string output = JsonConvert.SerializeObject(turn_DTO, Formatting.Indented);
            return output;
        }

        public string GetCardsLeft(string email)
        {
            Users user = CardsUsers_DB.GetInstance().GetUser(email)[0];
            int cardsLeft = CallDB.CountCardsLeft(user.ID);
            Message message = new Message();
            message.message = cardsLeft.ToString();
            string output = JsonConvert.SerializeObject(message, Formatting.Indented);
            return output;
        }

        private Match_Logic() { }
    }
}
