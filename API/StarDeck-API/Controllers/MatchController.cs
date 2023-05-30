using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StarDeck_API.DB_Calls;
using StarDeck_API.Models;

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
            try
            {

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
