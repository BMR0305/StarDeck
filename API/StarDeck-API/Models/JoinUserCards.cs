using Microsoft.EntityFrameworkCore;

namespace StarDeck_API.Models
{
    [Keyless]
    public class JoinUserCards
    {
        public string User_Key { get; set; }
        public string Card_Key { get; set; }
        public string Card_Image { get; set;}
        public string Card_Name { get; set; }
    }
}
