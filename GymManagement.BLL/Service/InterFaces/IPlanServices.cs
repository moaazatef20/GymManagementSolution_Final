using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.MembersVIewModels;
using GymManagement.BLL.ViewModels.PlansViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.BLL.Service.InterFaces
{
    public interface IPlanServices
    {
        Task<IEnumerable<PlanViewModel>> GetAllPlans(CancellationToken ct = default);

        Task<PlanViewModel?> GetPlanDetailsAsync(int id, CancellationToken ct = default);

        Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int id, CancellationToken ct = default);

        Task<Result> CreatePlanAsync(CreatePlanViewModel model, CancellationToken ct = default);

        Task<Result> UpdatePlanAsync(int id, UpdatePlanViewModel model, CancellationToken ct = default);

        Task<Result> TogglePlanStatusAsync(int id, CancellationToken ct = default);
    }
}
