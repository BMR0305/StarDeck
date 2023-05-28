using Microsoft.AspNetCore.Mvc;
using StarDeck_API.Models;
using Newtonsoft.Json;
using StarDeck_API.DB_Calls;
using StarDeck_API.Logic_Files;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StarDeck_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        //Context of the DB
        private readonly DBContext context;
        //Constructor of the class
        public CardController(DBContext context)
        {
            this.context = context;
            CardsUsers_DB.GetInstance().SetContext(context);
        }
        /*
         * Function that allows to post a new card
         * c: card that it's going to be posted
         * return: it returns Ok state if it succedes, and if it doesn't succed it returns the error  
         */
        [HttpPost]
        [Route("post")]
        public dynamic PostCard([FromBody] Card c)
        {
            try
            {
               
                CardsUsers_Logic.GetInstance().PostCard(c);
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
        * Function that allows to get n different random cards of m types 
        * num: number of different random cards
        * types: list of types allowed for the selection of the random cards. Format: ["type", "type2", ...] Example: ["R", "N"]
        * return: it returns the list of random cards selected in json format if it succeds, and if it doesn't succed it returns the error  
        */
        [HttpGet]
        [Route("getRandom/{num}")]

        public dynamic GetRandomCards(int num, [FromQuery] List<string> types)
        {
            try
            {

                string output = CardsUsers_Logic.GetInstance().GetRandomCardsSP(num, types);
                return output;               

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
         * Function that allows to get all the cards created in the DB.
         * return: if successful json with all the card items created, else a bad request error.
        */
        [HttpGet]
        [Route("getAll")]

        public dynamic GetAllCards()
        {
            try
            {
                string output = CardsUsers_Logic.GetInstance().GetAllCards();
                return output;

            } catch (Exception ex)
            {
                Message m = new Message();
                m.message = ex.Message;
                string output = JsonConvert.SerializeObject(m, Formatting.Indented);
                return output;
            }
        }

    }
}