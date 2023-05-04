using Microsoft.AspNetCore.Mvc;
using StarDeck_API.Models;
using StarDeck_API.Support_Components;

namespace StarDeck_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeckController : ControllerBase
    {
        private KeyGen KeyGenerator = KeyGen.GetInstance();

        //DB context
        private readonly DBContext context;
        public DeckController(DBContext context)
        {
            this.context = context;
        }
    }
}
