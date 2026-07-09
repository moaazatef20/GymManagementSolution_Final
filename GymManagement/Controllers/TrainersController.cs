using GymManagement.BLL.Service.Classes;
using GymManagement.BLL.Service.InterFaces;
using GymManagement.BLL.ViewModels.MembersVIewModels;
using GymManagement.BLL.ViewModels.TrainersViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class TrainersController : Controller
    {
        private readonly ITrainerServices _trainerServices;

        public TrainersController(ITrainerServices trainerServices)
        {
            _trainerServices = trainerServices;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var trainers = await _trainerServices.GetAllTrainers(ct);
            return View(trainers);
        }
        [HttpGet]
        public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainerViewModel model ,CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Create), model);
            var isCreated = await _trainerServices.CreateTrainerAsync(model, ct);
            if (isCreated.Success)
            {
                TempData["Success"] = "Trainer Created Successfully";
            }
            else
            {
                TempData["Failed"] = isCreated.ErrorMassage;
            }
            

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id,CancellationToken ct)
        {
            var TrainerDetails = await _trainerServices.GetTrainerDetailsAsync(id);
            if (TrainerDetails == null)
            {
                TempData["Failed"] = "Trainer NotFound";
            }
            return View(TrainerDetails);
        }

        public IActionResult Delete() => View();

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute]int id ,CancellationToken ct)
        { 
            var isDeleted = await _trainerServices.DeleteTrainerAsync(id,ct);

            if (isDeleted.Success)
            {
                TempData["Success"] = "Trainer deleted successfully";
            }
            else
            {
                TempData["Failed"] = isDeleted.ErrorMassage;
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var trainerDetails = await _trainerServices.GetTrainerToUpdateAsync(id, ct);
            if (trainerDetails is null)
            {
                TempData["Failed"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainerDetails);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, TrainerToUpdateViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Edit), model);
            var isUpdated = await _trainerServices.UpdateTrainerAsync(id,model);

            if (isUpdated.Success)
            {
                TempData["Success"] = "Member updated successfully";
            }
            else
            {
                TempData["Failed"] = isUpdated.ErrorMassage;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
