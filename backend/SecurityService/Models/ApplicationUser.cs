using Microsoft.AspNetCore.Identity;

namespace SecurityServiceBackend.Models
{
public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
    public string Gender { get; set; }
    public string Bio { get; set; }
    public int Age { get; set; }
    public List<string> Skills { get; set; } = new List<string>();
    public List<string> TopicsOfInterest { get; set; } = new List<string>();
    public string ImageUrl { get; set; }
}

}