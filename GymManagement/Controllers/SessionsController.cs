using GymManagement.BLL.Service.Classes;
using GymManagement.BLL.Service.InterFaces;
using GymManagement.BLL.ViewModels.SessionsViewModels;
using GymManagement.BLL.ViewModels.TrainersViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagement.PL.Controllers
{
    [Authorize]
    public class SessionsController : Controller
    {
        private readonly ISessionServices _sessionServices;

        public SessionsController(ISessionServices sessionServices)
        {
            _sessionServices = sessionServices;
        }
        private async Task TrainersAndCategoryDropDownAsync(CancellationToken ct)
        {
            ViewBag.Trainers = new SelectList(await _sessionServices.GetAllTrainersAsync(ct), "Id", "Name");
            ViewBag.Categories = new SelectList(await _sessionServices.GetAllCategorysAsync(ct), "Id", "CategoryName");
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var sessions = await _sessionServices.GetAllSessionsAsync(ct);
            return View(sessions);
        }
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await TrainersAndCategoryDropDownAsync(ct);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct )
        {
            if(!ModelState.IsValid)
            {
                await TrainersAndCategoryDropDownAsync(ct);
                return View(model);
            }

            var isCreated = await _sessionServices.CreateSessionAsync(model, ct);

            if(isCreated.Success)
            {
                TempData["Success"] = "Session Created Successfully";
            }
            else
            {
                TempData["Failed"] = isCreated.ErrorMassage;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var sessionDetails = await _sessionServices.GetSessionToUpdateAsync(id, ct);
            if (sessionDetails is null)
            {
                TempData["Failed"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            await TrainersAndCategoryDropDownAsync(ct);
            return View(sessionDetails);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, UpdateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Edit), model);
            var isUpdated = await _sessionServices.UpdateSessionAsync(id, model);

            if (isUpdated.Success)
            {
                TempData["Success"] = "Session updated successfully";
            }
            else
            {
                TempData["Failed"] = isUpdated.ErrorMassage;
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var sessionDetails = await _sessionServices.GetSessionDetailsAsync(id);
            if (sessionDetails == null)
            {
                TempData["Failed"] = "Session NotFound";
            }
            await TrainersAndCategoryDropDownAsync(ct);
            return View(sessionDetails);
        }

        public IActionResult Delete() => View();

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id, CancellationToken ct)
        {
            var isDeleted = await _sessionServices.DeleteSessionAsync(id, ct);

            if (isDeleted.Success)
            {
                TempData["Success"] = "Session deleted successfully";
            }
            else
            {
                TempData["Failed"] = isDeleted.ErrorMassage;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

