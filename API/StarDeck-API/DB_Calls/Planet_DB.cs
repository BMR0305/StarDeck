using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StarDeck_API.Models;
using System.Data;
using System.Diagnostics;

namespace StarDeck_API.DB_Calls
{
    public class Planet_DB
    {
        private static Planet_DB instance = null;
        private DBContext context;
        public static Planet_DB GetInstance()
        {
            if (instance == null)
            {
                instance = new Planet_DB();
            }
            return instance;
        }

        public void PostPlanet(Planet p)
        {
            try
            {
                context.planet.Add(p);
                context.SaveChanges();
            }
            catch (SqlException ex)
            {
                throw new Exception("SqlException: "+ex.Message);
            }
        }

        public List<Planet> GetAll()
        {
            List<Planet> planets = context.planet.ToList();
            if (planets.Count == 0)
            {
                throw new Exception("No planets found");
            }
            return planets;
        }

        public List<Planet> GetPlanetByName(string name)
        {
            Debug.WriteLine("Hi im getting the planet by name "+name);
            List<Planet> planetList = context.planet.FromSqlRaw("EXEC GetPlanet @name = {0}", name).ToList();
            if (planetList.Count == 0)
            {
                throw new Exception("No planet found");
            }
            return planetList;
        }

        /*
         * Function that obtains from a procedure in the DB the types of the planets as a string separated by '#'
         * Return: string with the types of the planets separated by '#'
         */
        public string GetTypes()
        {
            try
            {
                SqlParameter types = new SqlParameter("@types", SqlDbType.NVarChar, -1);
                types.Direction = ParameterDirection.Output;
                context.Database.ExecuteSqlRaw("EXEC GetTypes @types OUTPUT", types);
                if (types.Value == DBNull.Value)
                {
                    throw new Exception("No types found");
                }
                return types.Value.ToString();
            } 
            catch (SqlException ex)
            {
                throw new Exception("SqlException: "+ex.Message);
            }
        }

        public List<Planet> GetPlanetByID(string ID)
        {
            List<Planet> planet = context.planet.FromSqlRaw("EXEC GetPlanetByID @ID = {0}", ID).ToList();
            if (planet.Count == 0)
            {
                throw new Exception("No planet found");
            }
            return planet;
        }

        public List<Planet> GetGamePlanets()
        {
            List<Planet> planets = context.planet.FromSqlRaw("EXEC GetGamePlanets").ToList();
            if (planets.Count == 0)
            {
                throw new Exception("No planets found");
            }
            return planets;
        }

        /*
         * Function that sets the context of the DB to the class instance.
         * Params: context - context of the DB
         */
        public void SetContext(DBContext context)
        {
            this.context = context;
        }

        /*
         * Private constructor to avoid multiple instances of the class
         */
        private Planet_DB() { }
    }
}
