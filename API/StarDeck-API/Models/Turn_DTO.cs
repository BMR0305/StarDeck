using Microsoft.EntityFrameworkCore;

namespace StarDeck_API.Models
{
    public class Turn_DTO //Data Transfer Object
    {
        public string TurnID { get; set; }
        public int TurnNumber { get; set; }
        public int Energy { get; set; } //2*TurnNumber - 1
    }
}
