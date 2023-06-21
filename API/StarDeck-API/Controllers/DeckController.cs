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
        private KeyGen KeyGenerator = KeyGen.GetInstance();

        //DB context
        private readonly DBContext context;
        public DeckController(DBContext context)
        {
            this.context = context;
            Deck_DB.GetInstance().SetContext(context);
        }

        [HttpPost]
        [Route("post")]
        public dynamic PostDeck([FromBody] Deck_DTO d)
        {
            Deck_DB.GetInstance().SetContext(this.context);
            CardsUsers_DB.GetInstance().SetContext(this.context);
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
            Deck_DB.GetInstance().SetContext(this.context);
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
            Deck_DB.GetInstance().SetContext(this.context);
            try
            {
                string decks = Deck_Logic.GetInstance().GetPlayerDecks(email);
                return decks;
            }
            catch (Exception e)
            {
                Message m = new Message();
                m.message = e.Message;
                string output = JsonConvert.SerializeObject(m, Formatting.Indented);
                return output;
            }
        }
    }
}
