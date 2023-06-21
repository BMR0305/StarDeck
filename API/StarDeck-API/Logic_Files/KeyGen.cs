using StarDeck_API.Models;
using System.Runtime.CompilerServices;

namespace StarDeck_API.Logic_Files
{
    /*
     *  Singleton class for generating a unique key for a new user
     */
    public class KeyGen
    {
        private static KeyGen Instance = null;

        /*
         *  Method to get the instance of the KeyGen class
         */
        public static KeyGen GetInstance() 
        {
            
            if (Instance == null)
            {
                Instance = new KeyGen();
            }

            return Instance;
        }

        /*
         *  Method to generate a unique key for a new user
         *  Params: code - the first part of the key, indicates the object type of the key.
         *  Return: id - the unique key generated.
         */
        public string CreatePattern(string code)
        {
            string chars = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string id = "";
            Random rnd = new Random();
            id = code + new String(Enumerable.Range(0, 12).Select(n => chars[rnd.Next(chars.Length)]).ToArray());

            return id;
        }

        /*
         * Private constructor for the KeyGen class
         */
        private KeyGen() { }

    }
}
