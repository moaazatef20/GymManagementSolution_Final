using GymManagement.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymManagement.Models
{
    public class Plan : BaseEntity
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }= default!;
        
        [Required, MaxLength(150)]
        public string Description { get; set; } = default!;

        [Required , Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Required, Range(1, 365)]
        public int DurationDays { get; set; }

        public bool IsActive { get; set; } = true;
     

        public ICollection<Membership> Memberships { get; set; } = new List<Membership>();
    }
}
