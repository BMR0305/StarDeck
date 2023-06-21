using Newtonsoft.Json;
using StarDeck_API.DB_Calls;
using StarDeck_API.Models;
using System.Xml.Linq;

namespace StarDeck_API.Logic_Files
{
    public class Deck_Logic
    {
        private static Deck_Logic instance = null;
        private KeyGen KeyGenerator = KeyGen.GetInstance();
        private DBContext context;
        private Deck_DB CallDB = Deck_DB.GetInstance();

        public static Deck_Logic GetInstance()
        {
            if (instance == null)
            {
                instance = new Deck_Logic();
            }
            return instance;
        }

        public void PostDeck(Deck_DTO deck)
        {
            List<Deck> decks = CallDB.GetDecks();
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
            
            Users user = CardsUsers_DB.GetInstance().GetUser(deck.email_user)[0];

            Deck deck_toSave = new Deck();
            deck_toSave.Deck_ID = id;
            deck_toSave.d_name = deck.name;
            deck_toSave.Player_ID = user.ID;

            List<Deck_Card> cards = new List<Deck_Card>();
            for (int i = 0; i < deck.cards.Count; i++)
            {
                Deck_Card temp_card = new Deck_Card(); 
                temp_card.Deck_ID = id;
                temp_card.Card_ID = deck.cards[i].ID;
                cards.Add(temp_card);
            }

            CallDB.PostDeck(deck_toSave, cards);
        }

        public string GetDeck(string Deck_ID)
        {
            List<Card> cards = CallDB.GetDeckCards(Deck_ID);
            Deck deck = CallDB.GetDeck(Deck_ID);

            Deck_DTO deckDTO = new Deck_DTO();
            deckDTO.name = deck.d_name;
            deckDTO.code = Deck_ID;
            deckDTO.email_user = deck.Player_ID;
            deckDTO.cards = cards;
            string output = JsonConvert.SerializeObject(deckDTO, Formatting.Indented);
            return output;
        }

        /*
         * Method that creates the Deck_DTO based on the decks obtained from the DB corresponding to the email provided.
         * Params: email - user email
         * Return: JSON string with the decks in Deck_DTO format
         */
        public string GetPlayerDecks(string email)
        {
            List<Deck_DTO> deck_DTOs = new List<Deck_DTO>();
            List<Deck> decks = CallDB.GetPlayerDecks(email);

            for (int i = 0; i < decks.Count; i++)
            {
                Deck_DTO deck_DTO = new Deck_DTO();
                deck_DTO.name = decks[i].d_name;
                deck_DTO.code = decks[i].Deck_ID;
                deck_DTO.email_user = decks[i].Player_ID;
                deck_DTO.cards = CallDB.GetDeckCards(decks[i].Deck_ID);
                deck_DTOs.Add(deck_DTO);
            }

            string output = JsonConvert.SerializeObject(deck_DTOs, Formatting.Indented);
            return output;
        }

        /*
         * Function that sets the user's deck
         * Params: context - DBContext, id - deck id, email - user email
         */
        public void SetUserDeck(string id, string email)
        {
            CallDB.SetUserDeck(id, email);
        }

        private Deck_Logic() { }
    }
}
