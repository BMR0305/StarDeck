using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StarDeck_API.Models;
using System.Data;

namespace StarDeck_API.Support_Components
{
    public class Deck_DB
    {
        public static Deck_DB instance = null;

        private KeyGen KeyGenerator = KeyGen.GetInstance();

        public static Deck_DB GetInstance()
        {

            if (instance == null)
            {
                instance = new Deck_DB();
            }
            return instance;

        }

        public string PostDeck(Deck_Aux d, DBContext context)
        {
            try
            {
                List<Deck> decks = context.deck.ToList();
                string id = "";
                bool flag = true;

                if (decks.Count > 0)
                {
                    while (flag)
                    {
                        id = KeyGenerator.CreatePattern("D-");

                        for (int i = 0; i < decks.Count; i++)
                        {
                            if (decks[i].Deck_ID == id)
                            {
                                flag = true;
                                break;
                            }
                            else
                            {
                                flag = false;
                            }
                        }
                    }
                }
                else
                {
                    id = KeyGenerator.CreatePattern("D-");
                }

                var user = context.users.FromSqlRaw("EXEC GetPlayer @email = {0}",d.name_user).ToList();

                for (int i = 0; i < d.cards.Count; i++)
                {
                    Deck deck = new Deck();
                    deck.Deck_ID = id;
                    deck.Player_ID = user[0].ID;
                    deck.Card_ID = d.cards[i].ID;
                    deck.d_name = d.name;
                    context.deck.Add(deck);
                }

                context.SaveChanges();
                return "Saved Deck";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string GetDeck(DBContext context,string Deck_ID)
        {
            try
            {
                var user_id = new SqlParameter("@PlayerID", SqlDbType.NVarChar, -1);
                user_id.Direction = ParameterDirection.Output;
                var d_name = new SqlParameter("@d_name", SqlDbType.NVarChar, -1);
                d_name.Direction = ParameterDirection.Output;
                List<Card> deck_cards = context.cards.FromSqlRaw("EXEC GetDeckCards @deckID, @PlayerID OUTPUT, @d_name OUTPUT",
                                                                new SqlParameter("@deckID",Deck_ID),
                                                                user_id, d_name).ToList();
                Deck_Aux deck = new Deck_Aux();
                deck.name = d_name.Value.ToString();
                deck.code = Deck_ID;
                deck.name_user = user_id.Value.ToString();
                deck.cards = deck_cards;
                string output = JsonConvert.SerializeObject(deck, Formatting.Indented);
                return output;
                
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string GetPlayerDecks(DBContext context, string email)
        {
            try
            {
                var decks_aux = new List<Deck_Aux>();

                var player_decks = context.deckIDTable.FromSqlRaw("EXEC GetPlayerDecks @player_email = {0}", email).ToList();

                for (int i = 0; i < player_decks.Count; i++)
                {
                    var user_id = new SqlParameter("@PlayerID", SqlDbType.NVarChar, -1);
                    user_id.Direction = ParameterDirection.Output;
                    var d_name = new SqlParameter("@d_name", SqlDbType.NVarChar, -1);
                    d_name.Direction = ParameterDirection.Output;
                    List<Card> deck_cards = context.cards.FromSqlRaw("EXEC GetDeckCards @deckID, @PlayerID OUTPUT, @d_name OUTPUT",
                                                                      new SqlParameter("@deckID", player_decks[i].Deck_ID),
                                                                      user_id, d_name).ToList();
                    Deck_Aux deck = new Deck_Aux();
                    deck.name = d_name.Value.ToString();
                    deck.code = player_decks[i].Deck_ID;
                    deck.name_user = email;
                    deck.cards = deck_cards;
                    decks_aux.Add(deck);
                }

                string output = JsonConvert.SerializeObject(decks_aux, Formatting.Indented);
                return output;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private Deck_DB() { }
    }
}
