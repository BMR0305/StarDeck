using StarDeck_API.DB_Calls;
using StarDeck_API.Models;
using StarDeck_API.Logic_Files;
using Newtonsoft.Json;

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

        public void InitialTurn(string gameID, string email)
        {
            Users user = CardsUsers_DB.GetInstance().GetUser(email)[0];
            Partida game = CallDB.GetGameByID(gameID);

            Turn turn = new Turn();
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
            turn.Turn_ID = id;
            turn.Active_Player = game.Player1;
            turn.Game_ID = gameID;
            CallDB.InsertTurn(turn);
            CallDB.UpdateGameTurn(gameID, id);

        }

        public void CreateNewTurn(Turn turn)
        {
            CallDB.InsertTurn(turn);
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

            List<Card> cards = new List<Card>();
            
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

        public string TakeCard(string gameID, string email)
        {
            Partida game = CallDB.GetGameByID(gameID);
            Users user = CardsUsers_DB.GetInstance().GetUser(email)[0];

            CardsLeft cardtaken = CallDB.TakeCards(user.ID, 1)[0];
            CallDB.DeleteCardLeft(user.ID, cardtaken.Card_ID);
            Card card = CardsUsers_DB.GetInstance().GetCard(cardtaken.Card_ID);

            string output = JsonConvert.SerializeObject(card, Formatting.Indented);

            return output;
        }

        private Match_Logic() { }
    }
}
