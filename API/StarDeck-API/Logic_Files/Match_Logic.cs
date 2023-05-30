using StarDeck_API.DB_Calls;
using StarDeck_API.Models;

namespace StarDeck_API.Logic_Files
{
    public class Match_Logic
    {
        private static Match_Logic instance = null;
        private Match_DB CallDB = Match_DB.GetInstance;

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

        public void CreateNewTurn(Turn turn)
        {
            CallDB.InsertTurn(turn);
        }

        public void InsertDeckToCardsLeft(Partida game)
        {
            Users player1 = CardsUsers_DB.GetInstance().GetUserByID(game.Player1)[0];
            Users player2 = CardsUsers_DB.GetInstance().GetUserByID(game.Player2)[0];

            CallDB.InsertDeckToCardsLeft(player1.ID, player1.current_deck);
            CallDB.InsertDeckToCardsLeft(player2.ID, player2.current_deck);
        }

        public void GetInitialHand(string email)
        {

        }

        private Match_Logic() { }
    }
}
