using Data.Models.PKA;
using IntelliHome_Backend.Features.Shared.DTOs;

namespace IntelliHome_Backend.Features.PKA.DTOs
{
    public class WashingMachineCreationDTO : SmartDeviceDTO
    {
        public List<Guid> ModesIds{ get; set; }
        public WashingMachineCreationDTO() {
            ModesIds = new List<Guid>();
        }
    }
}
