using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GymManagement.BLL.ViewModels.PlansViewModels
{
    public class CreatePlanViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces")]
        public string Name { get; set; } = default!;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Description must be between 5 and 200 characters")]
        public string Description { get; set; } = default!;

        [Required(ErrorMessage = "Duration is required")]
        [Range(1, 365, ErrorMessage = "Duration must be between 1 and 365 days")]
        public int DurationDays { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.0, double.MaxValue, ErrorMessage = "Price cannot be a negative value")]
        public decimal Price { get; set; }

        public bool IsActive { get; set; }
    }
}