using Microsoft.AspNetCore.Mvc;
using StarDeck_API.Models;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StarDeck_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class User_CardController : ControllerBase
    {
        private readonly DBContext context;
        public User_CardController(DBContext context)
        {
            this.context = context;
        }
        // POST api/<UsersController>
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
