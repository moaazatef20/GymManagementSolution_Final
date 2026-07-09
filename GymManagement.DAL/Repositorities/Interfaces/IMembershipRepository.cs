using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.Repositorities.Interfaces
{
    public interface IMembershipRepository : IGenericRepository<Membership>
    {
        Task<IEnumerable<Membership>> GetAllMembershipsWithMemberAndPlanAsync(CancellationToken ct);
    }
}
