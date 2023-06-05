using StarDeck_API.DB_Calls;
using StarDeck_API.Models;
using StarDeck_API.Logic_Files;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using System.Data;

namespace StarDeck_API.Logic_Files
{
    public class Match_Logic
    {
        private static Match_Logic instance = null;
        private Match_DB CallDB = Match_DB.GetInstance;
        private KeyGen KeyGenerator = KeyGen.GetInstance();

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
            turn.Active_Player = game.Player1;
            turn.Game_ID = gameID;
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

        public string EndTurn(List<CardPlayed> cardsPlayed, string gameID, string email)
        {
            Partida game = CallDB.GetGameByID(gameID);
            Users user = CardsUsers_DB.GetInstance().GetUser(email)[0];
            int MaxTurns = 8;
            if (user.ID == game.Player2)
            {
                if (game.TurnCount <= MaxTurns)
                {
                    CallDB.SetTurnActivePlayer(game.C_Turn, "None");
                    Turn turn = new Turn();
                    turn.Turn_ID = CreateTurnID();
                    turn.Active_Player = game.Player1;
                    turn.Game_ID = gameID;
                    CallDB.InsertTurn(turn);
                    CallDB.UpdateGameTurn(gameID, turn.Turn_ID);
                    CallDB.CountTurn(gameID);
                } else
                {
                    //Logica de terminar partida
                }

            } else
            {
                CallDB.SetTurnActivePlayer(game.C_Turn, game.Player2);
            }

            if (cardsPlayed.Count > 0)
            {
                for (int i = 0; i < cardsPlayed.Count; i++)
                {
                    CallDB.InsertCardPlayed(cardsPlayed[i]);
                }
            }

            return "Turno terminado"; //Crear un game-state segun lo que pase, ya sea que termine la partida o no. La info del turno se solicita por aparte.
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
            string output = JsonConvert.SerializeObject(cardsPlayed, Formatting.Indented);
            return output;
        }

        public string EndGame(string gameID)
        {
            Partida game = CallDB.GetGameByID(gameID);
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
            } else if (score1 == score2)
            {
                CallDB.UpdateWinner("Tie", gameID);
                output = "";
            } else
            {
                CallDB.UpdateWinner(game.Player2, gameID);
                Users player = CardsUsers_DB.GetInstance().GetUserByID(game.Player2)[0];
                output = JsonConvert.SerializeObject(player, Formatting.Indented);
            }
            return output;
        }

        public string GetGameTurn(string gameID)
        {
            Partida game = CallDB.GetGameByID(gameID);
            string turnID = game.C_Turn;
            return turnID;
        }

        public string GetTurnActivePlayer(string gameID)
        {
            Partida game = CallDB.GetGameByID(gameID);
            string turnID = game.C_Turn;
            string activePlayer = CallDB.GetTurnByID(turnID).Active_Player;
            return activePlayer;
        }

        private Match_Logic() { }
    }
}
