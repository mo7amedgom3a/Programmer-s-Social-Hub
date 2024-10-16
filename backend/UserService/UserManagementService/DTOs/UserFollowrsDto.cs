namespace UserManagementService.DTOs
{
    public class UserFollowers
    {
        public string UserId { get; set; }
        public List<string> Followers { get; set; }
    }
}