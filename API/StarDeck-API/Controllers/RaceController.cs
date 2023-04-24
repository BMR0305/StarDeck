using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StarDeck_API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StarDeck_API.Controllers
{
    //Controller clase for the Races
    [Route("api/[controller]")]
    [ApiController]
    public class RaceController : ControllerBase
    {
        //Context of the DB
        private readonly DBContext context;
        //Constructor of the class
        public RaceController(DBContext context)
        {
            this.context = context;
        }
        /*
         * Function that allows to post a new race
         * r: race that it's going to be posted
         * return: it returns Ok state if it succedes, and if it doesn't succed it returns the error  
         */
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
        /*
         * Function that allows to get all races
         * return: it returns a list with all races in json format if it succedes, and if it doesn't succed it returns the error  
         */
        [HttpGet]
        [Route("getAll")]
        public dynamic getAll()
        {
            try
            {
                string output = JsonConvert.SerializeObject(context.race.ToArray(), Formatting.Indented);
                return output;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

    }
}
