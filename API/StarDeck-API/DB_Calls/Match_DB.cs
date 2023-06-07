using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StarDeck_API.Models;
using System.Data;
using System.Diagnostics;

namespace StarDeck_API.DB_Calls
{
    public class Match_DB
    {
        private static object lockObject = new object();
        private static Match_DB instance = null;
        private DBContext context;
        public static Match_DB GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new Match_DB();
                        }
                    }
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

        public void RefreshGameCache(Partida game)
        {
            lock (lockObject)
            try
            {
                context.Entry(game).Reload();
            }
            catch (SqlException e)
            {
                throw new Exception("Failed to refresh game cache: " + e.Message);
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

        public void UpdateGameTurn(string gameID, string turnID)
        {
            try
            {
                context.Database.ExecuteSqlRaw("EXEC SetGameTurn @gameID = {0}, @turnID = {1}", gameID, turnID);
                context.SaveChanges();
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
                context.SaveChanges();
            }
            catch (SqlException e)
            {
                throw new Exception("Failed to delete card: " + e.Message);
            }
        }

        public void AddPlayerReady(string turnID)
        {
            try
            {
                context.Database.ExecuteSqlRaw("EXEC AddPlayerReady @turnID = {0}", turnID);
                context.SaveChanges();
            }
            catch (SqlException e)
            {
                throw new Exception("Failed to count player ready: " + e.Message);
            }
        }

        public void InsertCardPlayed(CardPlayed cardPlayed)
        {
            try
            {
                context.cardPlayed.Add(cardPlayed);
                context.SaveChanges();
            }
            catch (SqlException e)
            {
                throw new Exception("Failed to insert card played: " + e.Message);
            }
        }

        public void CountTurn(string gameID)
        {
            try
            {
                context.Database.ExecuteSqlRaw("EXEC CountTurn @gameID = {0}", gameID);
                context.SaveChanges();
            }
            catch (SqlException e)
            {
                throw new Exception("Failed to count turn: " + e.Message);
            }
        }

        public List<CardPlayed> GetCardsPlayed(string gameID ,string turnID, string playerID)
        {
            try
            {
                List<CardPlayed> cardsPlayed = context.cardPlayed.FromSqlRaw("EXEC GetCardPlayedInTurn @gameID = {0}, @turnID = {1}, @playerID = {2}",
                                                                                                                   gameID, turnID, playerID).ToList();
                return cardsPlayed;
            }
            catch (SqlException e)
            {
                throw new Exception("Failed to get card played: " + e.Message);
            }
        }

        public List<CardPlayed> GetCardsPlayedFullGame(string playerID, string gameID)
        {
            CardsUsers_DB.GetInstance().SetContext(context);
            try
            {
                List<CardPlayed> cardsPlayed = context.cardPlayed.FromSqlRaw("EXEC GetCardsPlayed @playerID = {0}, @gameID = {1}",
                                                                                                       playerID, gameID).ToList();
                return cardsPlayed;
            }
            catch (SqlException e)
            {
                throw new Exception("Failed to get card played: " + e.Message);
            }
        }

        public int GetUserPoints(string card_list)
        {
            try
            {
                SqlParameter score = new SqlParameter("@pointsP", SqlDbType.Int);
                score.Direction = ParameterDirection.Output;
                SqlParameter cards = new SqlParameter("@cardID_list", card_list);
                context.Database.ExecuteSqlRaw("EXEC GetPlayerPoints @cardID_list, @pointsP OUTPUT",
                                                cards,score);
                try
                {
                    int scoreInt = Convert.ToInt32(score.Value);
                    return scoreInt;
                } catch (Exception e)
                {
                    int scoreInt = 0;
                    return scoreInt;
                }
            }
            catch (SqlException e)
            {
                throw new Exception("Failed to get user points: " + e.Message);
            }
        }

        public void UpdateWinner(string playerID, string gameID)
        {
            try
            {
                //Update winner updates the winner of the game and also the status of the game to finished represented by a 'T'
                context.Database.ExecuteSqlRaw("EXEC UpdateWinner @playerID = {0}, @gameID = {1}", playerID, gameID);
            }
            catch (SqlException e)
            {
                throw new Exception("Failed to update winner: " + e.Message);
            }
        }

        public Turn GetTurnByID(string turnID)
        {
            lock (lockObject)
            {
                try
                {
                    List<Turn> turn = context.turn.FromSqlRaw("EXEC GetTurnByID @turnID = {0}", turnID).ToList();
                    if (turn.Count == 0)
                    {
                        throw new Exception("Turn not found");
                    }
                    return turn[0];
                }
                catch (SqlException e)
                {
                    throw new Exception("Failed to get turn: " + e.Message);
                }
            }
        }

        public int CountCardsLeft(string playerID)
        {
            try
            {
                SqlParameter count = new SqlParameter("@cards_left", SqlDbType.Int);
                count.Direction = ParameterDirection.Output;
                SqlParameter player = new SqlParameter("@playerID", playerID);
                context.Database.ExecuteSqlRaw("EXEC CountCardsLeft @playerID, @cards_left OUTPUT", player, count);
                int countInt = Convert.ToInt32(count.Value);
                return countInt;
            }
            catch (SqlException e)
            {
                throw new Exception("Failed to get turn number: " + e.Message);
            }
        }

        public void EmptyCardsLeft(string playerID)
        {
            try
            {
                context.Database.ExecuteSqlRaw("EXEC EmptyCardsLeft @playerID = {0}", playerID);
                context.SaveChanges();
            }
            catch (SqlException e)
            {
                throw new Exception("Failed to empty cards left: " + e.Message);
            }
        }


        public void SetContext(DBContext context)
        {
            this.context = context;
        }

        private Match_DB() { }
    }
}
