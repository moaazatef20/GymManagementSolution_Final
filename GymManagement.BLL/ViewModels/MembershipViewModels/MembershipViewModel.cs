using System;

namespace GymManagement.BLL.ViewModels.MembershipViewModels
{
    public class MembershipViewModel
    {
        public int Id { get; set; }

        public int MemberId { get; set; }
        public string MemberName { get; set; } = default!;

        public int PlanId { get; set; }
        public string PlanName { get; set; } = default!;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Status => EndDate >= DateTime.Now ? "Active" : "Expired";
    }
}
