using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StarDeck_API.DB_Calls;
using StarDeck_API.Models;
using System.Data;

namespace StarDeck_API.Logic_Files
{
    /*
     * Singleton class that manages logic operations for the Cards & Users tables.
     */ 
    public class CardsUsers_Logic
    {
        //Constants
        private string UserDefaultAvatar = "data:image/jpeg;base64,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";
        private int InitialRanking = 0;
        private int InitialCoins = 20;

        //KeyGen object to generate the user keys
        private KeyGen KeyGenerator = KeyGen.GetInstance();

        //Instance of the class
        private static CardsUsers_Logic Instance = null;

        private CardsUsers_DB CallDB;
        public static CardsUsers_Logic GetInstance()
        {

            if (Instance == null)
            {                
                Instance = new CardsUsers_Logic();
            }

            return Instance;
        }

        /*
         * Method that saves a new user in the DB. Also generates a new ID for the user and checks if it's already in use.
         * Params: u - user to save.
         * Return: returns a string with the result of the operation or an exception.
         */
        public dynamic PostUser(Users u)
        {
            u.avatar = UserDefaultAvatar;
            u.ranking = InitialRanking;
            u.coins = InitialCoins;
            u.u_status = "A";
            bool flag = true;
            List<Users> users = CallDB.GetAllUsers();
            string id = "";
            u.current_deck = "";

            if (users.Count > 0)
            {

                while (flag)
                {
                    //Random rnd = new Random();
                    id = KeyGenerator.CreatePattern("U-");

                    for (int i = 0; i < users.Count; i++)
                    {
                        if (users[i].ID == id)
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
                id = KeyGenerator.CreatePattern("U-");
            }


            u.ID = id;

            string ret = CallDB.PostUser(u);
            return ret;
        }
        
        /*
         * Method that allows to get a random number of cards with different types from the procedure on the DB.
         * Params: context - context of the DB, num - number of cards to get, types - list of types of cards to get.
         */
        public string ValidateUser(string mail, string password)
        {
            string error = CallDB.ValidateUser(mail, password);            
            string output = JsonConvert.SerializeObject(error, Formatting.Indented);
            return output;
        }

        /*
         * Method that allows to get a random number of cards with different types from the procedure on the DB.
         * Params: context - context of the DB, num - number of cards to get.
         * Return: list of cards of the user in json format.
         */
        public dynamic GetUserCards(string mail)
        {

            List<Card> cards = CallDB.GetUserCards(mail);

            string output = JsonConvert.SerializeObject(cards.ToArray(), Formatting.Indented);
            return output;
        }

        /*
         * Method that allows to get a random number of cards with different types from the procedure on the DB.
         * Params: context - context of the DB, mail - email from the user to check if it has cards.
         * Return: boolean that indicates if the user has cards or not.
         */
        public bool HasCards(string mail)
        {
            int cardCount = CallDB.HasCardsSP(mail);
            if (cardCount > 0)
            {
                return true;
            }
            return false;
        }

        /* 
        * Function that allows to get the information of a user from the DB
        * Params: context - context of the DB, mail - mail of the user to get
        * Return: string with the information of the user in json format or a Message indicating that the user was not found
        */
        public string GetUser(string mail)
        {
            var userInfo = CallDB.GetUser(mail);
            string output = "";
            if (userInfo.Count == 0)
            {
                Message m = new Message();
                m.message = "User not found";
                output = JsonConvert.SerializeObject(m, Formatting.Indented);
                return output;
            }
            output = JsonConvert.SerializeObject(userInfo.ToArray(), Formatting.Indented);
            return output;
        }

        public bool Mail(string email)
        {
            List<Users> users = CallDB.GetAllUsers();
            bool ret;

            if (users.Count > 0)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].email == email)
                    {
                        ret = false;
                        return ret;
                    }
                }
                ret = true;
                return ret;
            }

            else
            {
                ret = true;
                return ret;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void PostCard(Card c)
        {
            c.c_status = "a";
            List<Card> cards = CallDB.GetAllCards();
            string id = "";
            bool flag = true;

            if (cards.Count > 0)
            {
                while (flag)
                {
                    //Random rnd = new Random();
                    id = KeyGenerator.CreatePattern("C-");

                    for (int i = 0; i < cards.Count; i++)
                    {
                        if (cards[i].ID == id)
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
                id = KeyGenerator.CreatePattern("C-");
            }


            c.ID = id;
            bool ret = CallDB.PostCard(c);

            if (!ret)
            {
                throw new Exception("Error saving card, something happened");
            }
        }
        public string GetAllCards()
        {
            List<Card> cards = CallDB.GetAllCards();
            string output = JsonConvert.SerializeObject(cards.ToArray(), Formatting.Indented);
            return output;
        }

        /*
         * Method that allows to get a random number of cards with different types from the procedure on the DB.
         * Params: num - number of cards to get, types - list of types of cards to get.
         */
        public string GetRandomCardsSP(int num, List<string> types)
        {

            string Types_String = "";

            for (int i = 0; i < types.Count; i++)
            {
                Types_String += types[i] + "#";
            }

            List<Card> RandomCards = CallDB.GetRandomCardsSP(num, Types_String);
            string output = JsonConvert.SerializeObject(RandomCards.ToArray(), Formatting.Indented);
            return output;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public void PostUserCard(string email, List<Card> cards)
        {
            User_Card u_c = new User_Card();
            List<Users> userInfo = CallDB.GetUser(email);
            if (userInfo.Count > 0)
            {
                string user_id = userInfo[0].ID;
                for (int i = 0; i < cards.Count; i++)
                {
                    u_c.user_key = user_id;
                    u_c.card_key = cards[i].ID;
                    bool ret = CallDB.PostUserCard(u_c);
                    if (!ret)
                    {
                        throw new Exception("Error inserting the card in the DB");
                    }
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /*
         * Private constructor for the DB_Procedures class
         */
        private CardsUsers_Logic()
        {
            this.CallDB = CardsUsers_DB.GetInstance();
        }
    }
}
