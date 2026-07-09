using System;

namespace GymManagement.BLL.ViewModels.BookingViewModels
{
    public class MemberForSessionViewModel
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; } = default!;

        public int SessionId { get; set; }
        public DateTime BookingDate { get; set; }
        public bool IsAttended { get; set; }
    }
}
