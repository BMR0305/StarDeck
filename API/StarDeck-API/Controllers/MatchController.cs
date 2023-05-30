using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarDeck_API.DB_Calls;
using StarDeck_API.Models;
using StarDeck_API.Logic_Files;

namespace StarDeck_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly DBContext context;
        public MatchController(DBContext context)
        {
            this.context = context;
            Match_DB.GetInstance.SetContext(context);
        }

        [HttpPost]
        [Route("NewTurn")]
        public dynamic NewTurn(Turn turn)
        {
            Match_DB.GetInstance.SetContext(this.context);
            CardsUsers_DB.GetInstance().SetContext(this.context);
            try
            {

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetHand/{gameID}/{email}")]
        public dynamic GetHand(string gameID, string email)
        {
            Match_DB.GetInstance.SetContext(context);
            CardsUsers_DB.GetInstance().SetContext(context);
            try
            {
                string output = Match_Logic.GetInstance.GetHand(gameID, email);
                return output;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("TakeCard/{gameID}/{email}")]
        public dynamic TakeCard(string gameID, string email)
        {
            try
            {
                string output = Match_Logic.GetInstance.TakeCard(gameID, email);
                return output;
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
