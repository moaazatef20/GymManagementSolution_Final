using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymManagement.DAL.Models
{
    public class Session : BaseEntity
    {
        [Required, MaxLength(200)]
        public string Description { get; set; } = null!;

        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }


        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    }
}
