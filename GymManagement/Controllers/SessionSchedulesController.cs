using GymManagement.BLL.Service.InterFaces;
using GymManagement.BLL.ViewModels.BookingViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagement.PL.Controllers
{
    [Authorize]
    public class SessionSchedulesController : Controller
    {
        private readonly IBookingServices _bookingServices;
        private readonly ISessionServices _sessionServices;

        public SessionSchedulesController(IBookingServices bookingServices, ISessionServices sessionServices)
        {
            _bookingServices = bookingServices;
            _sessionServices = sessionServices;
        }

        private async Task MembersDropDownAsync(CancellationToken ct)
        {
            ViewBag.Members = new SelectList(await _bookingServices.GetAllMembersAsync(ct), "Id", "Name");
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var sessions = await _sessionServices.GetAllSessionsAsync(ct);
            return View(sessions);
        }

        public async Task<IActionResult> GetMembersForUpcomingSession(int id, CancellationToken ct)
        {
            var members = await _bookingServices.GetBookingsForSessionAsync(id, ct);
            ViewData["SessionId"] = id;
            return View(members);
        }

        public async Task<IActionResult> GetMembersForOngoingSessions(int id, CancellationToken ct)
        {
            var members = await _bookingServices.GetBookingsForSessionAsync(id, ct);
            ViewData["SessionId"] = id;
            return View(members);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int id, CancellationToken ct)
        {
            await MembersDropDownAsync(ct);
            return View(new CreateBookingViewModel { SessionId = id });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await MembersDropDownAsync(ct);
                return View(model);
            }

            var isCreated = await _bookingServices.CreateBookingAsync(model, ct);

            if (isCreated.Success)
            {
                TempData["SuccessMessage"] = "Booking Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = isCreated.ErrorMassage;
            }
            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = model.SessionId });
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int MemberId, int SessionId, CancellationToken ct)
        {
            var isCancelled = await _bookingServices.CancelBookingAsync(MemberId, SessionId, ct);

            if (isCancelled.Success)
            {
                TempData["SuccessMessage"] = "Booking Cancelled Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = isCancelled.ErrorMassage;
            }
            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = SessionId });
        }

        [HttpPost]
        public async Task<IActionResult> Attended(int MemberId, int SessionId, CancellationToken ct)
        {
            var isMarked = await _bookingServices.MarkAttendedAsync(MemberId, SessionId, ct);

            if (isMarked.Success)
            {
                TempData["SuccessMessage"] = "Attendance Marked Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = isMarked.ErrorMassage;
            }
            return RedirectToAction(nameof(GetMembersForOngoingSessions), new { id = SessionId });
        }
    }
}
