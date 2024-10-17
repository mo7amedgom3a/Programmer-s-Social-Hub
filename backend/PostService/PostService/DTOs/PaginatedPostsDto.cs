namespace PostService.DTOs
{
    public class PaginatedPostsDto
    {
        public IEnumerable<PostDto> Posts { get; set; }
        public long TotalPosts { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
}