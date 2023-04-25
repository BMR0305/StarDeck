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
        //Context of the DB
        private readonly DBContext context;
        //Constructor of the class
        public CardController(DBContext context)
        {
            this.context = context;
        }
        /*
         * Function that allows to post a new card
         * c: card that it's going to be posted
         * return: it returns Ok state if it succedes, and if it doesn't succed it returns the error  
         */
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

                if (cards.Count > 0) {

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

                } else
                {
                    Random rnd = new Random();
                    id = "C-" + new String(Enumerable.Range(0, 12).Select(n => chars[rnd.Next(chars.Length)]).ToArray());
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

        /*
        * Function that allows to get n different random cards of m types 
        * num: number of different random cards
        * types: list of types allowed for the selection of the random cards. Format: ["type", "type2", ...] Example: ["R", "N"]
        * return: it returns the list of random cards selected in json format if it succeds, and if it doesn't succed it returns the error  
        */
        [HttpGet]
        [Route("getRandom/{num}")]

        public dynamic GetRandomCards(int num, [FromQuery] List<string> types)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest(ex.Message);

            }
            
        }

        /*
         * Function that allows to get all the cards created in the DB.
         * return: if successful json with all the card items created, else a bad request error.
        */
        [HttpGet]
        [Route("getAll")]

        public dynamic GetAllCards()
        {
            try
            {
                List<Card> cards = context.cards.ToList();
                string output = JsonConvert.SerializeObject(cards.ToArray(), Formatting.Indented);
                return output;

            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

    }
}