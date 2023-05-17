using Microsoft.AspNetCore.Mvc;
using StarDeck_API.Models;
using Newtonsoft.Json;
using StarDeck_API.Support_Components;
using Microsoft.EntityFrameworkCore;

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
        }

        [HttpGet]
        [Route("lookForGame/{email}")]
        public async Task<IActionResult> LookForGame(string email)
        {
            try
            {
                string output = await  Matchmaking.GetInstance().LookForGame(context, email);
                return Ok(new { message = output });
            }
            catch (System.Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet]
        [Route("cancelMM/{email}")]
        public dynamic CancelMM(string email)
        {
            try
            {
                string output = Matchmaking.GetInstance().CancelMM(context, email);
                return Ok(new { message = output });
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
