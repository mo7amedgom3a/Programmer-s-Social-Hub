using UserManagementService.Models;

namespace UserManagementService.DTOs
{
    public class PaginatedUsers
    {
        public IEnumerable<UserProfileDto> Items { get; set; }
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}