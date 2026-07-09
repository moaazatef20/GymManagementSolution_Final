using GymManagement.BLL.Service.InterFaces;
using GymManagement.BLL.ViewModels.MembersVIewModels;
using GymManagement.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    [Authorize]
    public class MembersController : Controller
    {
        private readonly IMemberServices _memberServices;
        private readonly IAttachementServices attachementServices;

        public MembersController(IMemberServices memberServices ,IAttachementServices attachementServices)
        {
            _memberServices = memberServices;
            this.attachementServices = attachementServices;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await _memberServices.GetAllMembers(ct);
            return View(members);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Create), model);

            var result = await _memberServices.CreateMemberAsync(model, ct);

            if (result.Success) 
            {
                TempData["Success"] = "Member Created Successfully";
            }
            else
            {
                TempData["Failed"] = result.ErrorMassage;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            var memberDetails = await _memberServices.GetMemberDetailsAsync(id, ct);
            if (memberDetails is null)
            {
                TempData["Failed"] = "Member not found";
                return RedirectToAction(nameof(Index));
            }
            return View(memberDetails);
        }

        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var healthDetails = await _memberServices.GetMemberHealthRecordAsync(id, ct);
            if (healthDetails is null)
            {
                TempData["Failed"] = "Member not found";
                return RedirectToAction(nameof(Index));
            }
            return View(healthDetails);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var member = await _memberServices.GetMemberDetailsAsync(id, ct);
            if (member is null)
            {
                TempData["Failed"] = "Member not found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id, CancellationToken ct)
        {
            var result = await _memberServices.DeleteMemberAsync(id, ct);
            if (result.Success)
            {
                TempData["Success"] = "Member deleted successfully";
            }
            else
            {
                TempData["Failed"] = result.ErrorMassage;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditMember(int id, CancellationToken ct)
        {
            var memberDetails = await _memberServices.GetMemberToUpdateAsync(id, ct);
            if (memberDetails is null)
            {
                TempData["Failed"] = "Member not found";
                return RedirectToAction(nameof(Index));
            }
            return View(memberDetails);
        }

        [HttpPost]
        public async Task<IActionResult> EditMember([FromRoute] int id, MemberToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(EditMember), model);

            var result = await _memberServices.UpdateMemberAsync(id, model, ct);
            if (result.Success)
            {
                TempData["Success"] = "Member updated successfully";
            }
            else
            {
                TempData["Failed"] = result.ErrorMassage;
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Picture(int id)
        {
            var member = await _memberServices.GetMemberDetailsAsync(id);
            if(member is null || string.IsNullOrEmpty(member.Photo))
                return NotFound();
            var result = attachementServices.GetFile(member.Photo, "MembersPhotos");
            if (result == null) return NotFound();
            return File(result.Value.stream,result.Value.contentType);

        }
    }
}