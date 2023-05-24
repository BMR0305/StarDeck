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
        //KeyGen object to generate the card keys
        private KeyGen KeyGenerator = KeyGen.GetInstance();

        //Context of the DB
        private readonly DBContext context;

        //Constructor of the class
        public PlanetController(DBContext context)
        {
            this.context = context;
            Planet_DB.GetInstance().SetContext(context);
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
               
                p.p_status = "a";
                List<Planet> planets = context.planet.ToList();
                string id = "";
                bool flag = true;
                if (planets.Count > 0)
                {
                    while (flag)
                    {
                        //Random rnd = new Random();
                        id = KeyGenerator.CreatePattern("P-");
                        for (int i = 0; i < planets.Count; i++)
                        {
                            if (planets[i].ID == id)
                            {
                                flag = true;
                                break;
                            }
                            else
                            {
                                flag = false;
                            }
                        }
                    }
                }
                else
                {
                    id = KeyGenerator.CreatePattern("P-");
                }
                p.ID = id;
                context.planet.Add(p);
                context.SaveChanges();
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
                List<Planet> planets = context.planet.ToList();
                string output = JsonConvert.SerializeObject(planets.ToArray(), Formatting.Indented);
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
                string output = Planet_Logic.GetInstance().GetPlanet(context,name);
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
