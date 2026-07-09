using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Service.InterFaces;
using GymManagement.BLL.ViewModels.MembersVIewModels;
using GymManagement.BLL.ViewModels.PlansViewModels;
using GymManagement.DAL.Models; 
using GymManagement.DAL.Repositorities.Interfaces;
using GymManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GymManagement.BLL.Service.Classes
{
    public class PlanServices : IPlanServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlanServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> CreatePlanAsync(CreatePlanViewModel model, CancellationToken ct = default)
        {
            if (await _unitOfWork.GetRepository<Plan>().AnyAsync(p => p.Name == model.Name, ct)) return Result.Validation("Plan name already exists");

            var plan = _mapper.Map<CreatePlanViewModel, Plan>(model);

            await _unitOfWork.GetRepository<Plan>().AddAsync(plan, ct);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Result.Ok() : Result.Fail("Failed to Create Plan");
        }

        public async Task<Result> TogglePlanStatusAsync(int id, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);

            if (plan is null)
                return Result.NotFound("Plan Not Found");

            plan.IsActive = !plan.IsActive;

            await _unitOfWork.GetRepository<Plan>().UpdateAsync(plan, ct);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Result.Ok() : Result.Fail("Failed to update plan status");
        }

        public async Task<IEnumerable<PlanViewModel>> GetAllPlans(CancellationToken ct = default)
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);
            if (!plans.Any()) return [];

            return _mapper.Map<IEnumerable<PlanViewModel>>(plans);
        }

        public async Task<PlanViewModel?> GetPlanDetailsAsync(int id, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
            if (plan is null) return null;

            return _mapper.Map<Plan, PlanViewModel>(plan);
        }

        public async Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int id, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
            if (plan is null) return null;

            return _mapper.Map<Plan, UpdatePlanViewModel>(plan);
        }

        public async Task<Result> UpdatePlanAsync(int id, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
            if (plan is null) return Result.NotFound("Plan Not Found");

            var nameExists = await _unitOfWork.GetRepository<Plan>().AnyAsync(p => p.Name == model.Name && p.Id != id, ct);
            if (nameExists) return Result.Validation("Plan name already exists");

            _mapper.Map(model, plan);

            await _unitOfWork.GetRepository<Plan>().UpdateAsync(plan, ct);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Result.Ok() : Result.Fail("Failed to Update Plan");
        }
    }
}