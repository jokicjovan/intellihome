namespace IntelliHome_Backend.Features.SPU.DTOs
{
    public class SprinklerSchedulerCreationDTO
    {
        public String Id { get; set; }
        public Boolean IsSpraying { get; set; }
        public String StartDate { get; set; }

        public String? EndDate { get; set; }
    }
}
