namespace CoreBot.BLL.Dto
{
    public class GameDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Developer { get; set; }
        public string Platform { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }    
        public string ImageUrl { get; set; }
    }

}
