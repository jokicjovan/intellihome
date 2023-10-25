using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models.Shared;

namespace Data.Models.SPU
{
    public class VehicleGate : IBaseEntity
    {
        public Guid Id { get; set; }
        public Boolean IsPublic { get; set; }
        public String CurrentLicencePlate { get; set; }
        public List<String> AllowedLicencePlates { get; set; }

        public VehicleGate()
        {
            AllowedLicencePlates = new List<String>();
        }

        public VehicleGate(Guid id, Boolean isPublic, String currentLicencePlate, List<String> allowedLicencePlates)
        {
            Id = id;
            IsPublic = isPublic;
            CurrentLicencePlate = currentLicencePlate;
            AllowedLicencePlates = allowedLicencePlates;
        }

    }
}
