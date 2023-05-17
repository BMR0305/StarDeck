using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StarDeck_API.Models;
using System.Data;

namespace StarDeck_API.Support_Components
{
    /*
     * Singleton class that grants access the stored procedures of the DB
     */ 
    public class CardsLogin_DB
    {
        //Instance of the class
        private static CardsLogin_DB Instance = null;
        public static CardsLogin_DB GetInstance()
        {

            if (Instance == null)
            {                
                Instance = new CardsLogin_DB();
            }

            return Instance;
        }
        /*
         * Method that allows to get a random number of cards with different types from the procedure on the DB.
         * Params: context - context of the DB, num - number of cards to get, types - list of types of cards to get.
         */ 
        public string GetRandomCardsSP(DBContext context, int num, List<string> types)
        {

            List<Card> RandomCards = new List<Card>();
            string Types_String = "";

            for (int i = 0; i < types.Count; i++)
            {
                Types_String += types[i] + "#";
            }

            var cards = context.cards.FromSqlRaw("EXEC GetRandomCards @num = {0}, @ctypeList = {1}", num, Types_String).ToList();
            RandomCards.AddRange(cards);
            string output = JsonConvert.SerializeObject(RandomCards.ToArray(), Formatting.Indented);
            return output;
        }
        
        /*
         * Method that allows to get a random number of cards with different types from the procedure on the DB.
         * Params: context - context of the DB, num - number of cards to get, types - list of types of cards to get.
         */
        public string ValidateUser(DBContext context, string mail, string password)
        {
            var errorMessage = new SqlParameter("@error_message", SqlDbType.NVarChar, -1);
            errorMessage.Direction = ParameterDirection.Output;
            var users = context.Database.ExecuteSqlRaw("EXEC ValidateUser @mail, @password, @error_message OUTPUT",
                                                    new SqlParameter("@mail",mail), 
                                                    new SqlParameter("@password", password), 
                                                    errorMessage);
            //string output = JsonConvert.SerializeObject(user.ToArray(), Formatting.Indented);
            string error = JsonConvert.SerializeObject(errorMessage.Value, Formatting.Indented);
            return error;
        }

        /*
         * Method that allows to get a random number of cards with different types from the procedure on the DB.
         * Params: context - context of the DB, num - number of cards to get.
         * Return: list of cards of the user in json format.
         */
        public string GetUserCards(DBContext context, string mail)
        {

            var cards = context.cards.FromSqlRaw("EXEC GetCards @email = {0}", mail).ToList();

            string output = JsonConvert.SerializeObject(cards.ToArray(), Formatting.Indented);
            return output;
        }

        /*
         * Method that allows to get a random number of cards with different types from the procedure on the DB.
         * Params: context - context of the DB, mail - email from the user to check if it has cards.
         * Return: boolean that indicates if the user has cards or not.
         */
        public bool HasCards(DBContext context, string mail)
        {
            var cards = context.cards.FromSqlRaw("EXEC GetCards @email = {0}", mail).ToList();
            if (cards.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /* 
        * Function that allows to get the information of a user from the DB
        * Params: context - context of the DB, mail - mail of the user to get
        * Return: string with the information of the user in json format or a Message indicating that the user was not found
        */
        public string GetUser(DBContext context, string mail)
        {
            var userInfo = context.users.FromSqlRaw("EXEC GetPlayer @email = {0}", mail).ToList();
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

        /*
         * Private constructor for the DB_Procedures class
         */
        private CardsLogin_DB() { }
    }
}
