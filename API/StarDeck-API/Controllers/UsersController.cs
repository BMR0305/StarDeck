using Microsoft.AspNetCore.Mvc;
using StarDeck_API.Models;

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
                u.avatar = "data:image/jpeg;base64,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";
                 
                u.ranking = 0;
                u.coins = 20;
                u.u_status = "a";
                string chars = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                bool flag = true;
                List<Users> users = context.users.ToList();
                string id = "";

                if (users.Count > 0) {

                    while (flag)
                    {
                        Random rnd = new Random();
                        id = "U-" + new String(Enumerable.Range(0, 12).Select(n => chars[rnd.Next(chars.Length)]).ToArray());

                        for (int i = 0; i < users.Count; i++)
                        {
                            if (users[i].ID == id)
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
                    Random rnd = new Random();
                    id = "U-" + new String(Enumerable.Range(0, 12).Select(n => chars[rnd.Next(chars.Length)]).ToArray());
                }


                u.ID = id;

                context.users.Add(u);
                context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("mail/{mail}")]
        public dynamic Mail(string mail) {

            List<Users> users = context.users.ToList();
            bool ret;

            if (users.Count > 0)
            {
                for (int i = 0; i <= users.Count; i++)
                {
                    if (users[i].email == mail)
                    {
                        ret = false;
                        return ret;
                    }
                }
                ret = true;
                return ret;
            } else
            {
                ret = true;
                return ret;
            }

        }

    }
}

