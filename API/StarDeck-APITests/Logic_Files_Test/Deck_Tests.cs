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
    public class Deck_Tests
    {
        private DBContext _dbContext;
        private string localDBConn = "UnitTestStarDeckDB";
        private KeyGen _keygen = KeyGen.GetInstance();
        public IConfigurationRoot Configuration { get; private set; }

        private DeckController _controllerD;
        private UsersController _controllerU;

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


            _controllerD = new DeckController(_dbContext);
            _controllerU = new UsersController(_dbContext);

        }

        [TestMethod()]
        public void PostDeckTest()
        {
            Deck_Logic deckLogic = Deck_Logic.GetInstance();
            Deck_DTO deckTest = new Deck_DTO
            {
                name = _keygen.CreatePattern(""),
                code = "",
                email_user = "unit_test@gmail.com",
                cards = new List<Card>
                        {
                            new Card
                            {
                                ID = "C-b7gFmeaOEi9x",
                                c_name = "lYkCulFRRiF1",
                                battle_pts = 1,
                                energy = 1,
                                c_image = "image",
                                c_type = "Rara",
                                race = "Raza 1",
                                c_status = "a",
                                c_description = "descripcion"
                            }
                         }
            };
            deckLogic.PostDeck(deckTest);
           
        }

        [TestMethod()]
        public void GetDeckTest()
        {
            Deck_Logic deckLogic = Deck_Logic.GetInstance();
            string deck_string = deckLogic.GetDeck("D-lpsrwabGiyq8");
            var deck = JsonConvert.DeserializeObject<Deck_DTO>(deck_string);
            Console.WriteLine(deck_string);
            Assert.IsTrue(deck != null);


        }

        [TestMethod()]
        public void GetPlayerDecksTest()
        {
            Deck_Logic deckLogic = Deck_Logic.GetInstance();
            string decks_string = deckLogic.GetPlayerDecks("unit_test@gmail.com");
            var decks = JsonConvert.DeserializeObject<List<Deck_DTO>>(decks_string);
            Console.WriteLine(decks_string);
            Assert.IsTrue(decks != null);


        }
        [TestMethod()]
        public void SetUserDecksTest()
        {
            Deck_Logic deckLogic = Deck_Logic.GetInstance();
            deckLogic.SetUserDeck("D-lpsrwabGiyq8", "unit_test@gmail.com");

        }


    }
}