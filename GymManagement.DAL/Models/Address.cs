using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymManagement.DAL.Models
{
    [Owned]
    public class Address
    {
        public int BuildingNo { get; set; } 
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
    }
}
