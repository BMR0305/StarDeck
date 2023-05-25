using Microsoft.AspNetCore.Mvc;
using StarDeck_API.Models;
using Newtonsoft.Json;
using StarDeck_API.Logic_Files;
using Microsoft.EntityFrameworkCore;

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

        public void SetContext(DBContext context)
        {
            this.context = context;
        }

        private Deck_DB() { }
    }
}
