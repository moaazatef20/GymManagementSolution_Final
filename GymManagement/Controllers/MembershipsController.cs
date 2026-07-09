using GymManagement.BLL.Service.InterFaces;
using GymManagement.BLL.ViewModels.MembershipViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagement.PL.Controllers
{
    [Authorize]
    public class MembershipsController : Controller
    {
        private readonly IMembershipServices _membershipServices;

        public MembershipsController(IMembershipServices membershipServices)
        {
            _membershipServices = membershipServices;
        }

        private async Task MembersAndPlansDropDownAsync(CancellationToken ct)
        {
            ViewBag.Members = new SelectList(await _membershipServices.GetAllMembersAsync(ct), "Id", "Name");
            ViewBag.Plans = new SelectList(await _membershipServices.GetAllActivePlansAsync(ct), "Id", "Name");
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var memberships = await _membershipServices.GetAllActiveMembershipsAsync(ct);
            return View(memberships);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await MembersAndPlansDropDownAsync(ct);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMembershipViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await MembersAndPlansDropDownAsync(ct);
                return View(model);
            }

            var isCreated = await _membershipServices.CreateMembershipAsync(model, ct);

            if (isCreated.Success)
            {
                TempData["SuccessMessage"] = "Membership Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = isCreated.ErrorMassage;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id, CancellationToken ct)
        {
            var isCancelled = await _membershipServices.CancelMembershipAsync(id, ct);

            if (isCancelled.Success)
            {
                TempData["SuccessMessage"] = "Membership Cancelled Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = isCancelled.ErrorMassage;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
