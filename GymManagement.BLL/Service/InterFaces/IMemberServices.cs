using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.MembersVIewModels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.BLL.Service.InterFaces
{
    public interface IMemberServices
    {
        Task<IEnumerable<MembersViewModels>> GetAllMembers(CancellationToken ct = default);

        Task<MembersViewModels?> GetMemberDetailsAsync(int id, CancellationToken ct = default);

        Task<HealthRecordViewModel> GetMemberHealthRecordAsync(int id, CancellationToken ct = default);

        Task<MemberToUpdateViewModel?> GetMemberToUpdateAsync(int id, CancellationToken ct = default);

        Task<Result> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct = default);

        Task<Result> UpdateMemberAsync(int id, MemberToUpdateViewModel model, CancellationToken ct = default);

        Task<Result> DeleteMemberAsync(int id, CancellationToken ct = default);

    }
}
