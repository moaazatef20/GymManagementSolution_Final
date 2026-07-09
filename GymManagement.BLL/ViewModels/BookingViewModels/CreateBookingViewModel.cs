using System.ComponentModel.DataAnnotations;

namespace GymManagement.BLL.ViewModels.BookingViewModels
{
    public class CreateBookingViewModel
    {
        [Required]
        public int SessionId { get; set; }

        [Required(ErrorMessage = "Member is required")]
        [Display(Name = "Member")]
        public int MemberId { get; set; }
    }
}
