namespace IntelliHome_Backend.Features.PKA.DTOs
{
    public class ACSchedulerCreationDTO
    {
        public String Id { get; set; }
        public Double Temperature { get; set; }
        public String Mode { get; set; }
        public String StartDate { get; set; }

        public String? EndDate { get; set; }
    }
}
