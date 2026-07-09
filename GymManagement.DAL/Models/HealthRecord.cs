using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymManagement.DAL.Models
{
    public class HealthRecord : BaseEntity
    {

        public decimal Height { get; set; }  
        public decimal Weight { get; set; }   

        [Required, MaxLength(5)]
        public string BloodType { get; set; } = null!;

        [MaxLength(500)]
        public string? Note { get; set; }

        // LastUpdate = UpdatedAt


        public int MemberId { get; set; }
        public Member Member { get; set; }
        
    }
}
