using Microsoft.AspNetCore.Mvc;
using StarDeck_API.Models;
using Newtonsoft.Json;
using StarDeck_API.Logic_Files;
using Microsoft.EntityFrameworkCore;

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
        }

        [HttpPost]
        [Route("post")]
        public dynamic PostDeck([FromBody] Deck_Aux d)
        {
            try
            {
                string Message = Deck_Logic.GetInstance().PostDeck(d,context);
                return Ok(new { message = Message});
            }
            catch (System.Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet]
        [Route("get/{id}")]
        public dynamic GetDeck(string id)
        {
            try
            {
                string deck = Deck_Logic.GetInstance().GetDeck(context, id);
                return Ok(deck);
            }
            catch (System.Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet]
        [Route("getPlayerDecks/{email}")]
        public dynamic GetPlayerDecks(string email)
        {
            try
            {
                string decks = Deck_Logic.GetInstance().GetPlayerDecks(context, email);
                return decks;
            }
            catch (System.Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}
