using GymManagement.BLL.Service.InterFaces;
using GymManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GymManagement.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IDashBoardServices dashBoardServices;

        public HomeController(ILogger<HomeController> logger, IDashBoardServices dashBoardServices)
        {
            this.logger = logger;
            this.dashBoardServices = dashBoardServices;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var data = await dashBoardServices.GetDashBoardDateAsync(ct);
            return View(data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
