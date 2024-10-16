using System.ComponentModel.DataAnnotations;
namespace UserManagementService.DTOs
{
    public class UserProfileUpdateDto
{
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9]{3,20}$")]
    public string Username { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public string Bio { get; set; }
    public int Age { get; set; }
    public List<string> Skills { get; set; }
    public List<string> TopicsOfInterest { get; set; }
    [RegularExpression(@"(http(s?):)([/|.|\w|\s|-])*\.(?:jpg|gif|png)")]
    public string ImageUrl { get; set; }
}

}