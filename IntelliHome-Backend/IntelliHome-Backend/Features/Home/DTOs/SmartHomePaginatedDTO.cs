namespace IntelliHome_Backend.Features.Home.DTOs
{
    public class SmartHomePaginatedDTO
    {
        public List<GetSmartHomeDTO> SmartHomes { get; set; }
        public Int32 TotalCount { get; set; }
    }
}
