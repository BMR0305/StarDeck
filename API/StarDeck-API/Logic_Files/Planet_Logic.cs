using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StarDeck_API.Models;
using System.Data;

namespace StarDeck_API.Logic_Files
{
    public class Planet_Logic
    {
        public static Planet_Logic instance = null;

        public static Planet_Logic GetInstance()
        {
            if (instance == null)
            {
                instance = new Planet_Logic();
            }
            return instance;
        }

        /**
         * Function that allows to get the information of a planet from the DB
         * Params: context - context of the DB, name - name of the planet to get
         * Return: string with the information of the planet in json format or a message indicating that the planet was not found
         */
        public string GetPlanet(DBContext context, string name)
        {
            string output = "";
            Message m = new Message();
            var planetInfo = context.planet.FromSqlRaw("EXEC GetPlanet @name = {0}", name).ToList();
            if (planetInfo.Count == 0)
            {
                m.message = "Planet not found";
                output = JsonConvert.SerializeObject(m, Formatting.Indented);
                return output;
            }
            output = JsonConvert.SerializeObject(planetInfo.ToArray(), Formatting.Indented);
            return output;
        }

        /**
         * Function that allows to get the information of all the planets from the DB
         * Params: context - context of the DB
         * Return: string with the information of all the planets in json format
         */
        public string GetAllPlanets(DBContext context)
        {
            var Planets = context.planet.ToList();
            string output = JsonConvert.SerializeObject(Planets.ToArray(), Formatting.Indented);
            return output;
        }

        /**
         * Function that allows to get the information of three random planets for a game from the DB
         * Params: context - context of the DB
         * Return: string with the planets selected for the game in json format
         */
        public string GetGamePlanets(DBContext context)
        {
            var Planets = context.planet.FromSqlRaw("EXEC GetGamePlanets").ToList();
            string output = JsonConvert.SerializeObject(Planets.ToArray(), Formatting.Indented);
            return output;
        }

        private Planet_Logic() { }
    }
}
