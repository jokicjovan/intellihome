namespace IntelliHome_Backend.Features.Users.DTOs
{
    public class UserDTO
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public String? Image { get; set; }
    }
}
