using GymManagement.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymManagement.DAL.Models
{
    public class GymUser : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;

        [RegularExpression(@"^(010|011|012|015)\d{8}$",
        ErrorMessage = "Egyptian phone format only.")]
        public string Phone { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Address Address { get; set; } = default!;
    }
 
}