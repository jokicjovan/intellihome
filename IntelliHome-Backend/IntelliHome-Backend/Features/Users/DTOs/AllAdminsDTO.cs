using Data.Models.Users;

namespace IntelliHome_Backend.Features.Users.DTOs
{
    public class AllAdminsDTO
    {
        public int TotalCount { get; set; }
        public IEnumerable<AdminDTO> Admins { get; set; }

        public AllAdminsDTO(IEnumerable<Admin> admins)
        {
            TotalCount = admins.Count();
            Admins = admins.Select(admin => new AdminDTO(admin.FirstName, admin.LastName, admin.Email));
        }
    }
}
