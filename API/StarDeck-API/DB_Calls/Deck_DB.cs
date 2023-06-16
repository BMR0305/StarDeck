using Microsoft.AspNetCore.Mvc;
using StarDeck_API.Models;
using Newtonsoft.Json;
using StarDeck_API.Logic_Files;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;

namespace StarDeck_API.DB_Calls
{
    public class Deck_DB
    {
        private static Deck_DB instance = null;
        private DBContext context;

        public static Deck_DB GetInstance()
        {
            if (instance == null)
            {
                instance = new Deck_DB();
            }
            return instance;
        }

        public void PostDeck(Deck deck, List<Deck_Card> cards)
        {
            try
            {
                context.deck.Add(deck);
                for (int i = 0; i < cards.Count; i++)
                {
                    context.deck_card.Add(cards[i]);
                }
                context.SaveChanges();
            }
            catch (SqlException e)
            {
                throw new ("Failed to Save Deck: "+e.Message);
            }
        }

        public List<Card> GetDeckCards(string Deck_ID)
        {
            
            List<Card> deck_cards = context.cards.FromSqlRaw("EXEC GetDeckCards @deckID",
                                                            new SqlParameter("@deckID", Deck_ID)).ToList();
            if (deck_cards.Count == 0)
            {
                throw new ("No cards found for given deck ID");
            }
            return deck_cards;
        }

        public Deck GetDeck(string Deck_ID)
        {
            Deck deck = context.deck.FromSqlRaw("EXEC GetDeckWithID @Deck_ID",
                                                        new SqlParameter("@Deck_ID", Deck_ID)).ToList()[0];
            if (deck == null)
            {
                throw new ("No deck found for given deck ID");
            }
            return deck;
        }

        public List<Deck> GetDecks()
        {
            return context.deck.ToList();
        }

        public List<Deck> GetPlayerDecks(string email)
        {
            List<Deck> decks = context.deck.FromSqlRaw("EXEC GetPlayerDecks @player_email = {0}", email).ToList();
            if (decks.Count == 0)
            {
                throw new ("No decks found for given email");
            }
            return decks;
        }

        public void SetUserDeck(string deckID, string email)
        {
            try
            {
                context.Database.ExecuteSqlRaw("EXEC SetDeck @deck_id = {0}, @email = {1}", deckID, email);
            }
            catch (SqlException e)
            {
                throw new ("Failed to set user deck: "+e.Message);
            }
        }

        public void SetContext(DBContext context)
        {
            this.context = context;
        }

        private Deck_DB() { }
    }
}
