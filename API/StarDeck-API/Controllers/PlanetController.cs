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
    public class PlanetController : ControllerBase
    {
        //Context of the DB
        private readonly DBContext context;
        private readonly ILogger<PlanetController> logger;
        //Constructor of the class
        public PlanetController(DBContext context, ILogger<PlanetController> logger)
        {
            this.context = context;
            Planet_DB.GetInstance().SetContext(context);
            this.logger = logger;
        }
        /*
         * Function that allows to post a new planet
         * p: planet that it's going to be posted
         * return: it returns Ok state if it succedes, and if it doesn't succed it returns the error  
         */
        [HttpPost]
        [Route("post")]
        public dynamic PostPlanet([FromBody] Planet p)
        {
            try
            {
                Planet_Logic.GetInstance().PostPlanet(p);
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
         * Function that allows to get all the planets
         * return: it returns Ok state if it succedes, and if it doesn't succed it returns bad request error  
         */
        [HttpGet]
        [Route("getAll")]
        public dynamic GetAllPlanet()
        {
            try
            {
                string output = Planet_Logic.GetInstance().GetAllPlanets();
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
         * Function that allows to get a planet by its name
         * name: name of the planet
         * return: it returns Ok state if it succeds, and if it doesn't succed it returns bad request error  
         */
        [HttpGet]
        [Route("get/{name}")]
        public dynamic GetPlanet(string name)
        {
            try
            {
                string output = Planet_Logic.GetInstance().GetPlanet(name);
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

        [HttpGet]
        [Route("getTypes")]
        public dynamic GetTypes()
        {
            try
            {
                string output = Planet_Logic.GetInstance().GetTypes();
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
    }
}
