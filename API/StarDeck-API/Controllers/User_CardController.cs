using Microsoft.AspNetCore.Mvc;
using StarDeck_API.Models;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StarDeck_API.Controllers
{
    //Controller class for the User_Card
    [Route("api/[controller]")]
    [ApiController]
    public class User_CardController : ControllerBase
    {
        //Context of the DB
        private readonly DBContext context;
        //Constructor of the class
        public User_CardController(DBContext context)
        {
            this.context = context;
        }
        /*
         * Function that allows to post a list of cards of one user
         * email: email of the user who owns the cards of the list
         * cards: List of cards that is going to be added to the user
         * return: it returns Ok state if it succedes, and if it doesn't succed it returns the error  
         */
        [HttpPost]
        [Route("post/{email}")]
        public dynamic PostUserCard(string email, [FromBody] List<Card> cards)
        {
            try
            {
                User_Card u_c = new User_Card();

                var user = context.users.FirstOrDefault(x => x.email == email);
                if (user == null)
                {
                    return "UserNotFound";
                }

                for (int i = 0; i < cards.Count; i++)
                {
                    u_c.user_key = user.ID;
                    u_c.card_key = cards[i].ID;

                    context.user_card.Add(u_c);
                    context.SaveChanges();
                }
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
