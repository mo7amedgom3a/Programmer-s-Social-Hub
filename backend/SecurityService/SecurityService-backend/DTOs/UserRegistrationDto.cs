namespace SecurityService.DTOs
{
    public class UserRegistrationDto
    {
        public string UserId { get; set; } // Unique ID from Security Service
        public string Username { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Bio { get; set; }
        public int Age { get; set; }
        public string[] Skills { get; set; }
        public string[] TopicsOfInterest { get; set; }
        public string ImageUrl { get; set; }
        public string Event { get; set; } // Event type, e.g., "UserRegistered"
    }
}