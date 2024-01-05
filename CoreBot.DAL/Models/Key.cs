namespace CoreBot.DAL.Models
{
    public class Key
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string KeyValue { get; set; }
        public string Status { get; set; }

        public Game Game { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
