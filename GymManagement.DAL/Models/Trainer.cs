using GymManagement.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.Models
{
    public class Trainer : GymUser
    {
        public Specialty Specialty { get; set; } 
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }

}
