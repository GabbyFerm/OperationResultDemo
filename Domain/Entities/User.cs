namespace Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!; // Store hashed password, never plain text

        public string Role { get; set; } = "User"; // Default role "User"
    }
}
