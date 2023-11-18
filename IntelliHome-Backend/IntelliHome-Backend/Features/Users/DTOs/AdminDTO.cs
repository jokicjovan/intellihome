namespace IntelliHome_Backend.Features.Users.DTOs
{
    public class AdminDTO
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String Email { get; set; }

        public AdminDTO(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}
