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
            Deck_Logic deck = Deck_Logic.GetInstance();
            String deck_string = deck.GetDeck(_dbContext, "D-bieeEbhPVVQR");
            Assert.IsTrue(deck_string.Contains("D-bieeEbhPVVQR"));
            
        }
        
        [TestMethod()] 
        public void PostDeckTest()
        {
            //DBContext context;
            Deck_Logic deck_instance = Deck_Logic.GetInstance();
            //crea un arreglo de Card
            List<Card> cards = new List<Card>();

            Card cards0 = new Card();
            cards0.ID = "C-89Pz8QGhwzNr";
            cards.Add(cards0);
            
            Card cards1 = new Card();
            cards1.ID = "C-BfqKgiGf1W3e";
            cards.Add(cards1);
            
            Card cards2 = new Card();
            cards2.ID = "C-CPpaD4h6cw0h";
            cards.Add(cards2);
            
            Card cards3 = new Card();
            cards3.ID = "C-Cr0thsJJexB8";
            cards.Add(cards3);
            
            Card cards4 = new Card();
            cards4.ID = "C-CuKea5Cs8qg0";
            cards.Add(cards4);
            
            Card cards5 = new Card();
            cards5.ID = "C-FQyGteAd2hfF";
            cards.Add(cards5);
            
            Card cards6 = new Card();
            cards6.ID = "C-hVT2M7mYoRrE";
            cards.Add(cards6);
            
            Card cards7 = new Card();
            cards7.ID = "C-jdOQw084WYkb";
            cards.Add(cards7);
            
            Card cards8 = new Card();
            cards8.ID = "C-juPVXvL8l3tn";
            cards.Add(cards8);
            
            Card cards9 = new Card();
            cards9.ID = "C-m4YaVFUdPOpw";
            cards.Add(cards9);
            
            Card cards10 = new Card();
            cards10.ID = "C-MMAp3aHO32VD";
            cards.Add(cards10);
            
            Card cards11 = new Card();
            cards11.ID = "C-Q8cBHJ8xJEaC";
            cards.Add(cards11);
            
            Card cards12 = new Card();
            cards12.ID = "C-Rh8C0osN9Utv";
            cards.Add(cards12);
            
            Card cards13 = new Card();
            cards13.ID = "C-rYQB6OMXSSQ0";
            cards.Add(cards13);
            
            Card cards14 = new Card();
            cards14.ID = "C-sHE6olr9zoly";
            cards.Add(cards14);
            
            Card cards15 = new Card();
            cards15.ID = "C-XO29iqnRZgY5";
            cards.Add(cards15);
            
            Card cards16 = new Card();
            cards16.ID = "C-yvkUaZUNOJMl";
            cards.Add(cards16);
            
            Card cards17 = new Card();
            cards17.ID = "C-z0Q8ZQ8xJEaC";
            cards.Add(cards17);

            Deck_DTO deck_aux = new Deck_DTO();
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
            Deck_Logic deck_instance = Deck_Logic.GetInstance();
            
            String deck_string = deck_instance.GetPlayerDecks(_dbContext, "U-3eykX6P25gvt");
            
            Assert.IsTrue(deck_string.Contains("D-"));
        }
        


    }
}
