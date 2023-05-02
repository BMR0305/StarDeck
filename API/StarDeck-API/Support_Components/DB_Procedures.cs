using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StarDeck_API.Models;

namespace StarDeck_API.Support_Components
{
    /*
     * Singleton class that grants access the stored procedures of the DB
     */ 
    public class DB_Procedures
    {
        //Instance of the class
        private static DB_Procedures Instance = null;
        public static DB_Procedures GetInstance()
        {

            if (Instance == null)
            {                
                Instance = new DB_Procedures();
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
         * Private constructor for the DB_Procedures class
         */
        private DB_Procedures() { }
    }
}
