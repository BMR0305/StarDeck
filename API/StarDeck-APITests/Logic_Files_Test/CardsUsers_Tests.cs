using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using StarDeck_API.Logic_Files;
using StarDeck_API.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StarDeck_API.Controllers;
namespace StarDeck_APITests.Logic_Files_Test
{

    [TestClass()]
    public class CardsUsers_Tests
    {
        private DBContext _dbContext;
        private string localDBConn = "UnitTestStarDeckDB";
        private KeyGen _keygen = KeyGen.GetInstance();
        public IConfigurationRoot Configuration { get; private set; }

        private UsersController _controller;

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


            _controller = new UsersController(_dbContext);

        }

        [TestMethod()]
        public void PostUsersTest()
        {
            CardsUsers_Logic cardUsersLogic = CardsUsers_Logic.GetInstance();
            Users userTest = new Users
            {

                avatar = "",
                ranking = 0,
                coins = 0,
                u_status = "",
                current_deck = "",
                ID = "",
                birthday = DateTime.Now,
                email = _keygen.CreatePattern("") + "@gmail.com",
                nickname = "unitTest",
                u_name = "Unit Test",
                nationality = "US",
                u_password = "ut123",
                u_type = "p"

            };
            String message = cardUsersLogic.PostUser(userTest);
            Assert.AreEqual(message, "Saved");

        }

        [TestMethod()]
        public void ValidateUserTest()
        {
            CardsUsers_Logic cardUsersLogic = CardsUsers_Logic.GetInstance();
            var validate_string = cardUsersLogic.ValidateUser("unit_test@gmail.com", "ut123");
            Assert.AreEqual(validate_string, "\"User found\"");

        }

        [TestMethod()]
        public void GetUserCardsTest()
        {
            CardsUsers_Logic cardUsersLogic = CardsUsers_Logic.GetInstance();
            string cards_string = cardUsersLogic.GetUserCards("unit_test@gmail.com");
            var cards = JsonConvert.DeserializeObject<List<Card>>(cards_string);
            Console.WriteLine(cards_string);
            Assert.IsTrue(cards != null);

        }

        [TestMethod()]
        public void HasCardsTest()
        {
            CardsUsers_Logic cardUsersLogic = CardsUsers_Logic.GetInstance();
            bool has_cards = cardUsersLogic.HasCards("unit_test@gmail.com");
            Assert.IsTrue(has_cards);
            //Assert.IsFalse(has_cards);

        }

        [TestMethod()]
        public void GetUserTest()
        {
            CardsUsers_Logic cardUsersLogic = CardsUsers_Logic.GetInstance();
            string user = cardUsersLogic.GetUser("unit_test@gmail.com");

            Assert.IsTrue(user != "User not found");

        }

        [TestMethod()]
        public void MailTest()
        {
            CardsUsers_Logic cardUsersLogic = CardsUsers_Logic.GetInstance();
            bool mail = cardUsersLogic.Mail("unit_test@gmail.com");
            Assert.IsTrue(!mail);

        }

        //////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////


        [TestMethod()]
        public void PostCardTest()
        {
            CardsUsers_Logic cardUsersLogic = CardsUsers_Logic.GetInstance();
            Card cardTest = new Card
            {
                c_name = _keygen.CreatePattern(""),
                battle_pts = 1,
                energy = 1,
                c_image = "image",
                c_type = "Rara",
                race = "Raza 1",
                c_status = "a",
                c_description = "descripcion"


            };
            cardUsersLogic.PostCard(cardTest);

        }
        [TestMethod()]
        public void GetAllCardsTest()
        {
            CardsUsers_Logic cardUsersLogic = CardsUsers_Logic.GetInstance();
            string cards_string = cardUsersLogic.GetAllCards();
            var cards = JsonConvert.DeserializeObject<List<Card>>(cards_string);
            Console.WriteLine(cards_string);
            Assert.IsTrue(cards != null);
        }

        [TestMethod()]
        public void GetRandomCardsSPTest()
        {
            CardsUsers_Logic cardUsersLogic = CardsUsers_Logic.GetInstance();
            List<string> rarezas = new List<string>
            {
                "Rara"
            };
            string cards_string = cardUsersLogic.GetRandomCardsSP(2, rarezas);
            var cards = JsonConvert.DeserializeObject<List<Card>>(cards_string);
            Console.WriteLine(cards_string);
            Assert.IsTrue(cards != null);
        }

        ////////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////////

        [TestMethod()]
        public void PostUserCardTest()
        {
            CardsUsers_Logic cardUsersLogic = CardsUsers_Logic.GetInstance();
            List<Card> cardsListTest = new List<Card>
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
            };
            cardUsersLogic.PostUserCard("unit_test@gmail.com", cardsListTest);

        }
    }
}