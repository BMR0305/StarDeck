namespace StarDeck_API.Models
{
    public class PartidaAux
    {
        public string ID { get; set; }
        public List<Users> Players { get; set; }
        public List<Planet> Planets { get; set; }
        public string p_status { get; set; } //En curso: EC, T: Terminada
    }
}
