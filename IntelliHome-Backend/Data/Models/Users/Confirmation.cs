using Data.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models.Users
{
    public class Confirmation : IBaseEntity
    {
        public Guid Id { get; set; }

        public int Code { get; set; }

        public DateTime ExpirationDate { get; set; }

        public User User { get; set; }
        public Confirmation() { }

    }
}
