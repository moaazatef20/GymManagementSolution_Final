using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.Models
{
    public class Member : GymUser
    {
        public string? Photo { get; set; }
        //Join Date = CreatedAt


        public HealthRecord? HealthRecord { get; set; }


        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

        public ICollection<Membership> Memberships { get; set; } = new List<Membership>();
    }
}
