namespace StarDeck_API.Models
{
    public class Deck_DTO //Deck DTO (Data Transfer Object)
    {
        public string name { get; set; }
        public string code { get; set; }
        public string email_user { get; set; }
        public List<Card> cards { get; set; }

    }
}
