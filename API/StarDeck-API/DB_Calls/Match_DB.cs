using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StarDeck_API.Models;

namespace StarDeck_API.DB_Calls
{
    public class Match_DB
    {
        private static Match_DB instance = null;
        private DBContext context;
        public static Match_DB GetInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Match_DB();
                }
                return instance;
            }
        }

        public void InsertDeckToCardsLeft(string playerID, string deckID)
        {
            try
            {
                context.Database.ExecuteSqlRaw("EXEC InsertDeckCardsLeft @deckID = {0}, @playerID = {1}", deckID, playerID);
            }
            catch (SqlException e)
            {
                throw new Exception("Failed to insert deck to cards left: " + e.Message);
            }
        }

        public void InsertTurn(Turn turn)
        {
            try
            {
                context.turn.Add(turn);
                context.SaveChanges();
            } 
            catch (SqlException e)
            {
                throw new Exception("Failed to insert turn: " + e.Message);
            }
        }

        public void InsertPlayerTurnCard(PlayerTurn_Card playerTurnCard)
        {
            try
            {
                context.playerTurn_Card.Add(playerTurnCard);
                context.SaveChanges();
            }
            catch (SqlException e)
            {
                throw new Exception("Failed to insert player turn card: " + e.Message);
            }
        }

        public Partida GetGameByID(string gameID)
        {
            try
            {
                var partida = context.partida.FromSqlRaw("EXEC GetGameByID @gameID = {0}", gameID).ToList()[0];
                if (partida == null)
                {
                    throw new Exception("Game not found");
                }
                return partida;
            }
            catch (SqlException e)
            {
                throw new Exception("Failed to get game: " + e.Message);
            }
        }

        public List<Turn> GetTurns()
        {
            try
            {
                return context.turn.ToList();
            }
            catch (SqlException e)
            {
                throw new Exception("Failed to get turns: " + e.Message);
            }
        }

        public void UpdateGameTurn(string turnID, string gameID)
        {
            try
            {
                context.Database.ExecuteSqlRaw("EXEC SetGameTurn @gameID = {0}, @turnID = {1}", gameID, turnID);
            }
            catch (SqlException e)
            {
                throw new Exception("Failed to update game turn: " + e.Message);
            }
        }

        public List<CardsLeft> TakeCards(string playerID, int num)
        {
            try
            {
                List<CardsLeft> hand = context.cardsLeft.FromSqlRaw("EXEC GetRandomFromDeck @playerID = {0}, @num = {1}", 
                                                                          playerID, num).ToList();
                if(hand == null)
                {
                    throw new Exception("Hand not found");
                }
                return hand;
            }
            catch (SqlException e)
            {
                throw new Exception("Failed to get hand: " + e.Message);
            }
        }

        public void DeleteCardLeft(string playerID, string cardID)
        {
            try
            {
                context.Database.ExecuteSqlRaw("EXEC EliminateCardLeft @playerID = {0}, @cardID = {1}", playerID, cardID);
            }
            catch (SqlException e)
            {
                throw new Exception("Failed to delete card: " + e.Message);
            }
        }

        public void SetContext(DBContext context)
        {
            this.context = context;
        }

        private Match_DB() { }
    }
}
