namespace StarDeck_API.Models
{
    public class Users
    {
        public string keyId { get; set; }
        public string email { get; set; }
        public string nickname { get; set; }
        public string name { get; set; }
        public DateTime birthday { get; set; }
        public string nationality { get; set; }
        public string password { get; set; }
        public bool status { get; set; }
        public string avatar { get; set; }
        public int ranking { get; set; }
        public int coins { get; set; }

    }
}
