using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using StarDeck_API.Logic_Files;
using StarDeck_API.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StarDeck_API.Controllers;
using System.Collections.Generic;

namespace StarDeck_APITests.Logic_Files_Test
{

    [TestClass()]
    public class Planet_Tests
    {
        private DBContext _dbContext;
        private string localDBConn = "UnitTestStarDeckDB";
        private KeyGen _keygen = KeyGen.GetInstance();
        public IConfigurationRoot Configuration { get; private set; }

        private PlanetController _controllerP;
        

        [TestInitialize]
        public void Initialize()
        {

            // Configurar una nueva instancia de StardeckDBContext para la base de datos de prueba


            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = configurationBuilder.Build();

            var options = new DbContextOptionsBuilder<DBContext>()
                .UseSqlServer(Configuration.GetConnectionString(localDBConn))
                .Options;

            _dbContext = new DBContext(options);


            _controllerP = new PlanetController(_dbContext);

        }

        [TestMethod()]
        public void PostDeckTest()
        {
            Planet_Logic planetLogic = Planet_Logic.GetInstance();
            Planet planetDeck = new Planet
            {
                p_name = _keygen.CreatePattern(""),
                p_image = "image",
                p_description = "description",
                p_effect = "effect",
                p_type = "Basico"
            };
            planetLogic.PostPlanet(planetDeck);

        }

        [TestMethod()]
        public void GetPlanetTest()
        {
            Planet_Logic planetLogic = Planet_Logic.GetInstance();
            string planet_string = planetLogic.GetPlanet("unitTesting");
            var planet = JsonConvert.DeserializeObject<List<Planet>>(planet_string);
            Console.WriteLine(planet_string);
            Assert.IsTrue(planet != null);


        }

        [TestMethod()]
        public void GetAllPlanetsTest()
        {
            Planet_Logic planetLogic = Planet_Logic.GetInstance();
            string planets_string = planetLogic.GetAllPlanets();
            var planets = JsonConvert.DeserializeObject<List<Planet>>(planets_string);
            Console.WriteLine(planets_string);
            Assert.IsTrue(planets != null);


        }

        [TestMethod()]
        public void GetGamePlanetsTest()
        {
            Planet_Logic planetLogic = Planet_Logic.GetInstance();
            string planets_string = planetLogic.GetGamePlanets(_dbContext);
            var planets = JsonConvert.DeserializeObject<List<Planet>>(planets_string);
            Console.WriteLine(planets_string);
            Assert.IsTrue(planets != null);

        }

        [TestMethod()]
        public void GetTypesTest()
        {
            Planet_Logic planetLogic = Planet_Logic.GetInstance();
            string types_String = planetLogic.GetTypes();
            var types = JsonConvert.DeserializeObject<List<String>>(types_String);
            Console.WriteLine(types_String);
            Assert.IsTrue(types != null);

        }
    }
}