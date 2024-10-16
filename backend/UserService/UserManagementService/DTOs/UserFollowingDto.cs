namespace UserManagementService.DTOs
{
    public class UserFollowingDto
    {
        public string UserId { get; set; }
        public List<string> Following { get; set; }
    }
}