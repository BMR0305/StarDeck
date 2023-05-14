using Microsoft.EntityFrameworkCore;

namespace StarDeck_API.Models
{
    [Keyless]
    public class DeckIDTable
    {
        public string d_name { get; set; }
        public string Deck_ID { get; set; }
        public string Player_ID { get;}
    }
}
