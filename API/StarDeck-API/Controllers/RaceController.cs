using Microsoft.AspNetCore.Mvc;
using StarDeck_API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StarDeck_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaceController : ControllerBase
    {
        private readonly DBContext context;
        public RaceController(DBContext context)
        {
            this.context = context;
        }
        // POST api/<UsersController>
        [HttpPost]
        [Route("post")]
        public dynamic PostRace([FromBody] Race r)
        {
            try
            {
                context.race.Add(r);
                context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

    }
}
