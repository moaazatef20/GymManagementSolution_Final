using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Service.InterFaces;
using GymManagement.BLL.ViewModels.MembershipViewModels;
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
    public class MembershipServices : IMembershipServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MembershipServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MembershipViewModel>> GetAllActiveMembershipsAsync(CancellationToken ct = default)
        {
            var memberships = await _unitOfWork.MembershipRepository.GetAllMembershipsWithMemberAndPlanAsync(ct);
            var activeMemberships = memberships.Where(m => m.EndDate >= DateTime.Now);
            return _mapper.Map<IEnumerable<MembershipViewModel>>(activeMemberships);
        }

        public async Task<IEnumerable<MemberSelectViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);
            if (!members.Any()) return [];

            return _mapper.Map<IEnumerable<MemberSelectViewModel>>(members);
        }

        public async Task<IEnumerable<PlanSelectViewModel>> GetAllActivePlansAsync(CancellationToken ct = default)
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);
            var activePlans = plans.Where(p => p.IsActive);
            return _mapper.Map<IEnumerable<PlanSelectViewModel>>(activePlans);
        }

        public async Task<Result> CreateMembershipAsync(CreateMembershipViewModel model, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(model.MemberId, ct);
            if (member is null) return Result.NotFound("Member Not Found");

            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(model.PlanId, ct);
            if (plan is null) return Result.NotFound("Plan Not Found");

            if (!plan.IsActive) return Result.Validation("Only active plans can be assigned");

            var hasActiveMembership = await _unitOfWork.GetRepository<Membership>()
                .AnyAsync(m => m.MemberId == model.MemberId && m.EndDate >= DateTime.Now, ct);
            if (hasActiveMembership) return Result.Validation("Member already has an active membership");

            var startDate = DateTime.Now;
            var membership = new Membership
            {
                MemberId = model.MemberId,
                PlanId = model.PlanId,
                StartDate = startDate,
                EndDate = startDate.AddDays(plan.DurationDays)
            };

            await _unitOfWork.GetRepository<Membership>().AddAsync(membership, ct);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Result.Ok() : Result.Fail("Failed to Create Membership");
        }

        public async Task<Result> CancelMembershipAsync(int memberId, CancellationToken ct = default)
        {
            var membership = await _unitOfWork.GetRepository<Membership>()
                .FirstOrDefaultAsync(m => m.MemberId == memberId && m.EndDate >= DateTime.Now, ct);

            if (membership is null) return Result.Validation("No active membership found for this member");

            await _unitOfWork.GetRepository<Membership>().DeleteAsync(membership, ct);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Result.Ok() : Result.Fail("Failed to Cancel Membership");
        }
    }
}
