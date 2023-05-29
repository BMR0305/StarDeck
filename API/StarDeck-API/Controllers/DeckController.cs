using Microsoft.AspNetCore.Mvc;
using StarDeck_API.Models;
using Newtonsoft.Json;
using StarDeck_API.Logic_Files;
using Microsoft.EntityFrameworkCore;
using StarDeck_API.DB_Calls;

namespace StarDeck_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class DeckController : ControllerBase
    {
        private readonly ILogger<DeckController> logger;
        //DB context
        private readonly DBContext context;
        public DeckController(DBContext context, ILogger<DeckController> logger)
        {
            this.context = context;
            this.logger = logger;
            Deck_DB.GetInstance().SetContext(context);
        }

        [HttpPost]
        [Route("post")]
        public dynamic PostDeck([FromBody] Deck_DTO d)
        {
            try
            {
                Deck_Logic.GetInstance().PostDeck(d);
                return Ok();
            }
            catch (System.Exception e)
            {
                Message m = new Message();
                m.message = e.Message;
                string output = JsonConvert.SerializeObject(m, Formatting.Indented);
                return output;
            }
        }

        [HttpGet]
        [Route("get/{id}")]
        public dynamic GetDeck(string id)
        {
            try
            {
                string deck = Deck_Logic.GetInstance().GetDeck(id);
                return Ok(deck);
            }
            catch (System.Exception e)
            {
                Message m = new Message();
                m.message = e.Message;
                string output = JsonConvert.SerializeObject(m, Formatting.Indented);
                return output;
            }
        }

        [HttpGet]
        [Route("getPlayerDecks/{email}")]
        public dynamic GetPlayerDecks(string email)
        {
            try
            {
                string decks = Deck_Logic.GetInstance().GetPlayerDecks(email);
                return decks;
            }
            catch (System.Exception e)
            {
                Message m = new Message();
                m.message = e.Message;
                string output = JsonConvert.SerializeObject(m, Formatting.Indented);
                return output;
            }
        }
    }
}
