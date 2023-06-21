namespace StarDeck_API.Models
{
    public class Partida
    {
        public string ID { get; set; }
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public string Winner { get; set; }
        public string Planet1 { get; set; }
        public string Planet2 { get; set; }
        public string Planet3 { get; set; }
        public string p_status { get; set; } //En curso: EC, T: Terminada
        public string C_Turn { get; set; }
        public int TurnCount { get; set; }
    }
}
