using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace SecurityServiceBackend.DTOs
{
public class UserDTO
{
    [Required]
   [RegexStringValidator(@"^[a-zA-Z0-9]{3,20}$")]
    public string Username { get; set; }
    [Required]
    [RegexStringValidator(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$")]
    public string Password { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public string Bio { get; set; }
    public int Age { get; set; }
    public List<string> Skills { get; set; }
    public List<string> TopicsOfInterest { get; set; }
   // [RegexStringValidator(@"(http(s?):)([/|.|\w|\s|-])*\.(?:jpg|gif|png)")] // only allow jpg, gif, png
    public string ImageUrl { get; set; }
}

}