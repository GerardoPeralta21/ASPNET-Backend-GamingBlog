namespace WebApiGames.DTO.BlogHeader
{
    public class BlogHeaderCreateDTO
    {
        public int BlogId { get; set; } // Required foreign key property
        public string Title { get; set; }
    }
}
