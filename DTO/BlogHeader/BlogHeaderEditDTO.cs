namespace WebApiGames.DTO.BlogHeader
{
    public class BlogHeaderEditDTO
    {
        public int Id { get; set; }
        public int BlogId { get; set; } // Required foreign key property
        public string Title { get; set; }
    }
}
