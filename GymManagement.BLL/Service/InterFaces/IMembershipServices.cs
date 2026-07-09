using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.MembershipViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.BLL.Service.InterFaces
{
    public interface IMembershipServices
    {
        Task<IEnumerable<MembershipViewModel>> GetAllActiveMembershipsAsync(CancellationToken ct = default);
        Task<IEnumerable<MemberSelectViewModel>> GetAllMembersAsync(CancellationToken ct = default);
        Task<IEnumerable<PlanSelectViewModel>> GetAllActivePlansAsync(CancellationToken ct = default);

        Task<Result> CreateMembershipAsync(CreateMembershipViewModel model, CancellationToken ct = default);
        Task<Result> CancelMembershipAsync(int memberId, CancellationToken ct = default);
    }
}
