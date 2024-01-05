
namespace CoreBot.DAL.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Developer { get; set; }
        public string Platform { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<Key> Keys { get; set; } = new List<Key>();
    }
}