using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarDeck_API.Logic_Files;
using StarDeck_API.Models;

namespace StarDeck_APITests.Support_Components
{

    [TestClass()]
    public class Planet_DBTests
    {
        private StarDeck_API.Models.DBContext _dbContext;

        [TestInitialize]
        public void Setup()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<StarDeck_API.Models.DBContext>()
                .UseInMemoryDatabase("MiBaseDeDatosEnMemoria")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            _dbContext = new StarDeck_API.Models.DBContext(options);
        }
        
        [TestMethod()]
        public void GetPlanetTest()
        {
            //DBContext context;
            Planet_Logic planet = Planet_Logic.GetInstance();
            String planet_string = planet.GetPlanet(_dbContext, "Europa");
            Assert.IsTrue(planet_string.Contains("Europa"));
        } 
        
        [TestMethod()]
        public void GetAllPlanetsTest()
        {
            //DBContext context;
            Planet_Logic planet = Planet_Logic.GetInstance();
            String planet_string = planet.GetAllPlanets(_dbContext);
            Assert.AreEqual(planet_string,"[]");
        }
        
        [TestMethod()]
        public void GetGamePlanetsTest()
        {
            //DBContext context;
            Planet_Logic planet = Planet_Logic.GetInstance();
            String planet_string = planet.GetGamePlanets(_dbContext);
            Assert.AreEqual(planet_string,"[]");
        }

    }
}