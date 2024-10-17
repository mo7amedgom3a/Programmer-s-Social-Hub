using MongoDB.Bson.Serialization.Attributes;

namespace PostService.DTOs
{
    public class CreatePostDto
    {
        public string AuthorId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Code { get; set; }
        public string Language { get; set; }
        public List<string> Images { get; set; } = new List<string>();
    }
}
