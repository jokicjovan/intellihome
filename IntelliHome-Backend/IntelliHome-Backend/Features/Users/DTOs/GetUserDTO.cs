using Data.Models.Users;

namespace IntelliHome_Backend.Features.Users.DTOs
{
    public class GetUserDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string? Image { get; set; }

        public GetUserDTO(User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Username = user.Username;
            Image = user.Image;
        }
    }
}
