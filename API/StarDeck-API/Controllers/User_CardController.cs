using Microsoft.AspNetCore.Mvc;
using StarDeck_API.Models;
using StarDeck_API.DB_Calls;
using Newtonsoft.Json;
using StarDeck_API.Logic_Files;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StarDeck_API.Controllers
{
    //Controller class for the User_Card
    [Route("api/[controller]")]
    [ApiController]
    public class User_CardController : ControllerBase
    {
        //Context of the DB
        private readonly DBContext context;
        //Constructor of the class
        public User_CardController(DBContext context)
        {
            this.context = context;
            CardsUsers_DB.GetInstance().SetContext(context);
        }
        /*
         * Function that allows to post a list of cards of one user
         * email: email of the user who owns the cards of the list
         * cards: List of cards that is going to be added to the user
         * return: it returns Ok state if it succedes, and if it doesn't succed it returns the error  
         */
        [HttpPost]
        [Route("post/{email}")]
        public dynamic PostUserCard(string email, [FromBody] List<Card> cards)
        {
            try
            {
                CardsUsers_Logic.GetInstance().PostUserCard(email, cards);
                return Ok();
            }
            catch (Exception ex)
            {
                Message m = new Message();
                m.message = ex.Message;
                string output = JsonConvert.SerializeObject(m, Formatting.Indented);
                return output;
            }
        }

        /*
         * Function that allows to check if an user has cards
         * email: email of the user to check if it has cards
         * return: If success returns a flag, else returns an error  
         */
        [HttpGet]
        [Route("HasCards/{email}")]
        public dynamic HasCards(string email)
        {
            try
            {
                bool flag = CardsUsers_Logic.GetInstance().HasCards(email);
                return flag;
            }

            catch (Exception ex)
            {
                Message m = new Message();
                m.message = ex.Message;
                string output = JsonConvert.SerializeObject(m, Formatting.Indented);
                return output;
            }
        }

        /*
         * Function that allows to get all the cards of one user
         * email: email of the user who owns the cards
         * return: it returns Ok state if it succedes, and if it doesn't succed it returns the error  
         */
        [HttpGet]
        [Route("GetAllCards/{email}")]
        public dynamic GetAllCards(string email)
        {
            try
            {
                var cards = CardsUsers_Logic.GetInstance().GetUserCards(email);
                return cards;
            }
            catch (Exception ex)
            {
                Message m = new Message();
                m.message = ex.Message;
                string output = JsonConvert.SerializeObject(m, Formatting.Indented);
                return output;
            }
        }

    }
}
