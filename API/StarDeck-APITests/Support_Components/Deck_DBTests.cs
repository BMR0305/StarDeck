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
    public class Deck_DBTests
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
        public void GetDeckTest()
        {
            //DBContext context;
            Deck_DB deck = Deck_DB.GetInstance();
            String deck_string = deck.GetDeck(_dbContext, "D-bieeEbhPVVQR");
            Assert.IsTrue(deck_string.Contains("D-bieeEbhPVVQR"));
            
        }
        
        [TestMethod()] 
        public void PostDeckTest()
        {
            //DBContext context;
            Deck_DB deck_instance = Deck_DB.GetInstance();
            //crea un arreglo de Card
            List<Card> cards = new List<Card>();
            
            cards.Add(new Card());
            
            cards[0] = new Card();
            cards[0].ID = "C-89Pz8QGhwzNr";
            cards[1] = new Card();
            cards[1].ID = "C-BfqKgiGf1W3e";
            cards[2] = new Card();
            cards[2].ID = "C-CPpaD4h6cw0h";
            cards[3] = new Card();
            cards[3].ID = "C-Cr0thsJJexB8";
            cards[4] = new Card();
            cards[4].ID = "C-CuKea5Cs8qg0";
            cards[5] = new Card();
            cards[5].ID = "C-FQyGteAd2hfF";
            cards[6] = new Card();
            cards[6].ID = "C-hVT2M7mYoRrE";
            cards[7] = new Card();
            cards[7].ID = "C-jdOQw084WYkb";
            cards[8] = new Card();
            cards[8].ID = "C-juPVXvL8l3tn";
            cards[9] = new Card();
            cards[9].ID = "C-m4YaVFUdPOpw";
            cards[10] = new Card();
            cards[10].ID = "C-MMAp3aHO32VD";
            cards[11] = new Card();
            cards[11].ID = "C-Q8cBHJ8xJEaC";
            cards[12] = new Card();
            cards[12].ID = "C-Rh8C0osN9Utv";
            cards[13] = new Card();
            cards[13].ID = "C-rYQB6OMXSSQ0";
            cards[14] = new Card();
            cards[14].ID = "C-sHE6olr9zoly";
            cards[15] = new Card();
            cards[15].ID = "C-XO29iqnRZgY5";
            cards[16] = new Card();
            cards[16].ID = "C-yvkUaZUNOJMl";
            cards[17] = new Card();
            cards[17].ID = "C-ZC64gJ0PweRF";
            
            Deck_Aux deck_aux = new Deck_Aux();
            deck_aux.name = "holaprofe";
            deck_aux.cards = cards;
            deck_aux.code = "";
            deck_aux.name_user = "U-3eykX6P25gvt";
            
            String deck_string = deck_instance.PostDeck(deck_aux, _dbContext);
            
            Assert.IsTrue(deck_string.Contains("D-"));
            
        }
        
        [TestMethod()] 
        public void GetPlayerDecks()
        {
            //DBContext context;
            Deck_DB deck_instance = Deck_DB.GetInstance();
            
            String deck_string = deck_instance.GetPlayerDecks(_dbContext, "U-3eykX6P25gvt");
            
            Assert.AreEqual(deck_string, "<<Query root of type 'FromSqlQueryRootExpression' wasn't handled by provider code. This issue happens when using a provider specific method on a different provider where it is not supported.>>");

        }

    }
}
