using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StarDeck_API.Models;
using System.Data;

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

        public List<Planet> GetPlanetByName(string name)
        {
            return context.planet.FromSqlRaw("EXEC GetPlanet @name = {0}", name).ToList();
        }

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

        public void SetContext(DBContext context)
        {
            this.context = context;
        }
        private Planet_DB() { }
    }
}
