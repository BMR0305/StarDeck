using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StarDeck_API.DB_Calls;
using StarDeck_API.Models;
using StarDeck_API.Logic_Files;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StarDeck_API.Controllers
{
    //Controller clase for the Users
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //Context of the DB
        private readonly DBContext context;
        private readonly ILogger<UsersController> logger;

        //Constructor of the class
        public UsersController(DBContext context, ILogger<UsersController> logger)
        {
            this.context = context;
            CardsUsers_DB.GetInstance().SetContext(context);
            this.logger = logger;
        }

        /*
         * Function that allows to post a new User
         * u: User that it's going to be posted
         * return: it returns Ok state if it succedes, and if it doesn't succed it returns the error  
         */
        [HttpPost]
        [Route("post")]
        public dynamic PostUsers([FromBody] Users u)
        {
            try
            {
                string ret = CardsUsers_Logic.GetInstance().PostUser(u);
                if (ret == "saved")
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Not saved");
                }
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
         * Function to verify if user's mail is already registered or not
         * mail: Mail to verify
         * return: Boolean verifying mail
         */
        [HttpGet]
        [Route("mail/{mail}")]
        public dynamic Mail(string mail) {

            try
            {
                bool ret = CardsUsers_Logic.GetInstance().Mail(mail);
                return ret;
            }
            catch (Exception ex)
            {
                Message message = new Message();
                message.message = ex.Message;
                return message.message;
            }

        }

        /*
         * Function that validates the user through the procedure ValidateUser in the DB
         * Params: data, consists in an array that contains the mail and the password of the user
         * Return: Ok state if it succedes, and if it doesn't succed it returns the error
         */
        [HttpGet]
        [Route("login")]
        public dynamic UserValidation([FromQuery] List<string> data)
        {
            try
            {
                string output = CardsUsers_Logic.GetInstance().ValidateUser(data[0], data[1]);
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
        * Function that allows to get a user by its email
        * Params: email, consists in the email of the user
        * Return: User associated to the email if it succedes, and if it doesn't succed it returns the error
        */

        [HttpGet]
        [Route("get/{email}")]
        public dynamic GetUser(string email)
        {
            try
            {
                string output = CardsUsers_Logic.GetInstance().GetUser(email);
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
         * Function that sets game deck to a user
         * Params: --id, consists in the id of the deck, --email, consists in the email of the user
         * Return Message if it succedes, and if it doesn't succed it returns the error
         */

        [HttpGet]
        [Route("setDeck/{id}/{email}")]
        public dynamic SetID(string id, string email)
        {
            try
            {
                Deck_Logic.GetInstance().SetUserDeck(id, email);
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
    }
}

