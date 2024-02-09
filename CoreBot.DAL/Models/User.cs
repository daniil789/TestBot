namespace CoreBot.DAL.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Channel { get; set; }
        public bool IsAdmin { get; set; }

        public List<Order> Orders { get; set; }
    }
}