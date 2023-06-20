namespace StarDeck_API.Models
{
    public class CardPlayed_DTO
    {
        public string GameID { get; set; }
        public Card Card { get; set; }
        public string PlayerID { get; set; }
        public string Planet { get; set; }
        public string Turn { get; set; }
    }
}
