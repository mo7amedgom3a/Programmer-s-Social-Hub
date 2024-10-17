namespace PostService.DTOs
{
    public class UpdatePostDto
    {
        public string AuthorId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Code { get; set; }
        public List<string> Images { get; set; } = new List<string>();
    }
}
