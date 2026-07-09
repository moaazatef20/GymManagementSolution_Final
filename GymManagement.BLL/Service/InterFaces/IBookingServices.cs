using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.BookingViewModels;
using GymManagement.BLL.ViewModels.MembershipViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.BLL.Service.InterFaces
{
    public interface IBookingServices
    {
        Task<IEnumerable<MemberSelectViewModel>> GetAllMembersAsync(CancellationToken ct = default);
        Task<IEnumerable<MemberForSessionViewModel>> GetBookingsForSessionAsync(int sessionId, CancellationToken ct = default);

        Task<Result> CreateBookingAsync(CreateBookingViewModel model, CancellationToken ct = default);
        Task<Result> CancelBookingAsync(int memberId, int sessionId, CancellationToken ct = default);
        Task<Result> MarkAttendedAsync(int memberId, int sessionId, CancellationToken ct = default);
    }
}
