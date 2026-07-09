using GymManagement.BLL.Service.InterFaces;
using GymManagement.BLL.ViewModels.PlansViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagement.Controllers
{
    [Authorize]
    public class PlansController : Controller
    {
        private readonly IPlanServices _planServices;

        public PlansController(IPlanServices planServices)
        {
            _planServices = planServices;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await _planServices.GetAllPlans(ct);
            return View(plans);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var planDetails = await _planServices.GetPlanDetailsAsync(id, ct);

            if (planDetails == null)
            {
                TempData["Failed"] = "Plan not found";
                return RedirectToAction(nameof(Index));
            }
            return View(planDetails);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreatePlanViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _planServices.CreatePlanAsync(model, ct);

            if (result.Success)
            {
                TempData["Success"] = "Plan Created Successfully";
            }
            else
            {
                TempData["Failed"] = result.ErrorMassage;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var planDetails = await _planServices.GetPlanToUpdateAsync(id, ct);

            if (planDetails is null)
            {
                TempData["Failed"] = "Plan not found";
                return RedirectToAction(nameof(Index));
            }
            return View(planDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, UpdatePlanViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _planServices.UpdatePlanAsync(id, model, ct);

            if (result.Success)
            {
                TempData["Success"] = "Plan updated successfully";
            }
            else
            {
                TempData["Failed"] = result.ErrorMassage;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Activate(int id, CancellationToken ct)
        {
            var result = await _planServices.TogglePlanStatusAsync(id, ct);

            if (result.Success)
            {
                TempData["Success"] = "Plan status has been updated successfully.";
            }
            else
            {
                TempData["Failed"] = result.ErrorMassage;
            }

            return RedirectToAction(nameof(Index)); 
        }
    }
}