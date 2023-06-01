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
    public class MatchmakingController : Controller
    {
        private readonly DBContext context;
        
        public MatchmakingController(DBContext context)
        {
            this.context = context;
            Matchmaking_DB.GetInstance().SetContext(context);
        }

        [HttpGet]
        [Route("lookForGame/{email}")]
        public async Task<IActionResult> LookForGame(string email)
        {
            Matchmaking_DB.GetInstance().SetContext(this.context);
            CardsUsers_DB.GetInstance().SetContext(this.context);
            Planet_DB.GetInstance().SetContext(this.context);
            Match_DB.GetInstance.SetContext(this.context);
            Deck_DB.GetInstance().SetContext(this.context);
            try
            {
                string output = await  Matchmaking_Logic.GetInstance().LookForGame(context, email);
                return Ok(output);
            }
            catch (Exception ex)
            {
                Message m = new Message();
                m.message = ex.Message;
                string output = JsonConvert.SerializeObject(m, Formatting.Indented);
                return StatusCode(500,output);
            }
        }

        [HttpGet]
        [Route("cancelMM/{email}")]
        public dynamic CancelMM(string email)
        {
            try
            {
                string output = Matchmaking_Logic.GetInstance().CancelMM(context, email);
                return Ok(new { message = output });
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
