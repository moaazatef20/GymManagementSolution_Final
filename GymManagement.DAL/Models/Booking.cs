using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.Models
{
    public class Booking :BaseEntity
    {
        public bool IsAttended { get; set; } = false;

        public int MemberId { get; set; }
        public Member Member { get; set; }


        public int SessionId { get; set; }
        public Session Session { get; set; }
    }
}
