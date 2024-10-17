using PostService.DTOs;

namespace PostService.DTOs
{
    public class CreateCommentDto
    {
        public string AuthorId { get; set; }
        public string Content { get; set; }
        public string Language { get; set; }
        public string Code { get; set; }
    }
}