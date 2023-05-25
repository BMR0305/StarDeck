namespace StarDeck_API.Models
{
    public class Partida_DTO //Partida DTO (Data Transfer Object)
    {
        public string ID { get; set; }
        public List<Users> Players { get; set; }
        public List<Planet> Planets { get; set; }
        public string p_status { get; set; } //En curso: EC, T: Terminada
    }
}
