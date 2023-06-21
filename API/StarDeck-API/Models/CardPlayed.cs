namespace StarDeck_API.Models
{
    public class CardPlayed
    {
        public string GameID { get; set; }
        public string CardID { get; set; }
        public string PlayerID { get; set; }
        public string Planet { get; set; }
        public string Turn { get; set; }
    }
}
