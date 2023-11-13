using Data.Models.Shared;

namespace Data.Models.PKA
{
    public class WashingMachineMode : IBaseEntity
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public Int32 Duration { get; set; }
        public Int32 Temperature { get; set; }
    }
}
