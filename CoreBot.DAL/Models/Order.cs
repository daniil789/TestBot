namespace CoreBot.DAL.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int KeyId { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }

        public User User { get; set; }
        public Key Key { get; set; }
    }
}