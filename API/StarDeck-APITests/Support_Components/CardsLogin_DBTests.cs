using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarDeck_API.Support_Components;
using StarDeck_API.Models;

namespace StarDeck_APITests.Support_Components
{

    [TestClass()]
    public class CardsLogin_DBTests
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
        public void GetRandomCardsSPTest()
        {
            CardsLogin_DB cardsLogin = CardsLogin_DB.GetInstance();
            String cards_string = cardsLogin.GetRandomCardsSP(_dbContext, 5, new List<string> { "Rara", "Normal" });
            Assert.IsTrue(cards_string.Contains("Rara"));
        }
        
        [TestMethod()]
        public void ValidateUserTest()
        {
            CardsLogin_DB cardsLogin = CardsLogin_DB.GetInstance();
            String cards_string = cardsLogin.ValidateUser(_dbContext, "qwer", "qwer1234");
            Assert.AreEqual(cards_string,"true");
        }
        
        [TestMethod()]
        public void GetUserCardsTest()
        {
            CardsLogin_DB cardsLogin = CardsLogin_DB.GetInstance();
            String cards_string = cardsLogin.GetUserCards(_dbContext, "prueba@44.com");
            Assert.AreEqual(cards_string,"false");
        }
        
        [TestMethod()]
        public void HasCards()
        {
            CardsLogin_DB cardsLogin = CardsLogin_DB.GetInstance();
            Boolean cards_boolen = cardsLogin.HasCards(_dbContext, "prueba@44.com");
            Assert.IsTrue(cards_boolen);
        }

    }
}