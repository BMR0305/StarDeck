using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StarDeck_API.DB_Calls;
using StarDeck_API.Models;
using System.Data;

namespace StarDeck_API.Logic_Files
{
    public class Planet_Logic
    {
        private static Planet_Logic instance = null;
        private KeyGen KeyGenerator = KeyGen.GetInstance();
        private Planet_DB CallDB = Planet_DB.GetInstance();

        public static Planet_Logic GetInstance()
        {
            if (instance == null)
            {
                instance = new Planet_Logic();
            }
            return instance;
        }

        public void PostPlanet(Planet planet)
        {
            planet.p_status = "a";
            List<Planet> planets = CallDB.GetAll();
            string id = "";
            bool flag = true;
            if (planets.Count > 0)
            {
                while (flag)
                {
                    //Random rnd = new Random();
                    id = KeyGenerator.CreatePattern("P-");
                    for (int i = 0; i < planets.Count; i++)
                    {
                        if (planets[i].ID == id)
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
                id = KeyGenerator.CreatePattern("P-");
            }
            planet.ID = id;
        }

        /**
         * Function that allows to get the information of a planet from the DB
         * Params: name - name of the planet to get
         * Return: string with the information of the planet in json format or a message indicating that the planet was not found
         */
        public string GetPlanet(string name)
        {
            var planetInfo = CallDB.GetPlanetByName(name);
            string output = JsonConvert.SerializeObject(planetInfo.ToArray(), Formatting.Indented);
            return output;
        }

        /**
         * Function that allows to get the information of all the planets from the DB
         * Params: context - context of the DB
         * Return: string with the information of all the planets in json format
         */
        public string GetAllPlanets()
        {
            var Planets = CallDB.GetAll();
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

        /**
         * Function that  returns the types of planets that are in the DB as a string separated by #
         * Return: string with the types of planets in json format.
         */
        public string GetTypes()
        {
            string types = CallDB.GetTypes();
            string[] typesList = types.Split('#');
            string output = JsonConvert.SerializeObject(typesList.ToArray(), Formatting.Indented);
            return output;
        }

        /*
         * Private constructor to avoid multiple instances of the class
         */
        private Planet_Logic() { }
    }
}
