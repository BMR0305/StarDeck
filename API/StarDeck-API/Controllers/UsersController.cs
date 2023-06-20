using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StarDeck_API.DB_Calls;
using StarDeck_API.Models;
using StarDeck_API.Logic_Files;
using Microsoft.EntityFrameworkCore;

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

        //Constructor of the class
        public UsersController(DBContext context)
        {
            this.context = context;
            CardsUsers_DB.GetInstance().SetContext(context);
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
            CardsUsers_DB.GetInstance().SetContext(this.context);
            try
            {
                string ret = CardsUsers_Logic.GetInstance().PostUser(u);
                if (ret == "Saved")
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

            CardsUsers_DB.GetInstance().SetContext(this.context);
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
            CardsUsers_DB.GetInstance().SetContext(this.context);
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
            CardsUsers_DB.GetInstance().SetContext(this.context);
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
            CardsUsers_DB.GetInstance().SetContext(this.context);
            Deck_DB.GetInstance().SetContext(this.context);
            string output = "";
            Message m = new Message();
            try
            {
                Deck_Logic.GetInstance().SetUserDeck(id, email);
                m.message = "Deck " + id + "set for: " + email;
                output = JsonConvert.SerializeObject(m, Formatting.Indented);
                return Ok(m);
            }
            catch (Exception ex)
            {
                m.message = ex.Message;
                output = JsonConvert.SerializeObject(m, Formatting.Indented);
                return output;
            }
        }
    }
}

