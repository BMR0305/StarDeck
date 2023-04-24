namespace StarDeck_API.Models
{
    public class Users
    {
        public string ID { get; set; }
        public string email { get; set; }
        public string nickname { get; set; }
        public string u_name { get; set; }
        public DateTime birthday { get; set; }
        public string nationality { get; set; }
        public string u_password { get; set; }
        public string u_status { get; set; }
        public string avatar { get; set; }
        public int ranking { get; set; }
        public int coins { get; set; }

        public string u_type { get; set; }

    }
}
