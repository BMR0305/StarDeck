using Microsoft.AspNetCore.Mvc;
using StarDeck_API.Models;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StarDeck_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly DBContext context;
        public CardController(DBContext context)
        {
            this.context = context;
        }
        // POST api/<UsersController>
        [HttpPost]
        [Route("post")]
        public dynamic PostCard([FromBody] Card c)
        {
            try
            {
               
                c.c_status = "a";
                string chars = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
                
                List<Card> cards = context.cards.ToList();
                string id = "";
                bool flag = true;
                while (flag)
                {
                    Random rnd = new Random();
                    id = "C-" + new String(Enumerable.Range(0, 12).Select(n => chars[rnd.Next(chars.Length)]).ToArray());

                    for (int i = 0; i < cards.Count; i++)
                    {
                        if (cards[i].ID == id)
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

                c.ID = id;

                context.cards.Add(c);
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
        [Route("getRandom/{num}")]

        public string GetRandomCards(int num, [FromQuery] List<string> types)
        {
            List<Card> cards = context.cards.ToList();
            List<Card> cardsType = new List<Card>();
            bool flag = false;

            for (int i = 0; i < cards.Count; i++)
            {
                for (int j = 0; j < types.Count; j++)
                {
                    if (cards[i].c_type == types[j])
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                    cardsType.Add(cards[i]);
                }
                flag = false;
            }

            if (num <= cardsType.Count)
            {
                List<Card> returnCards = new List<Card>();

                for (int k = 0; k < num;)
                {
                    Random rnd = new Random();
                    int ind = rnd.Next(0, cardsType.Count);

                    if (!returnCards.Contains(cardsType[ind]))
                    {
                        returnCards.Add(cardsType[ind]);
                        k++;
                    }
                }
                string output = JsonConvert.SerializeObject(returnCards.ToArray(), Formatting.Indented);
                return output;
            }

            else
            {
               return "Not enough cards of specify types";
            }
        }

    }
}