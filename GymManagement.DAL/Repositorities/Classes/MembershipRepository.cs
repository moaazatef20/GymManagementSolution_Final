using GymManagement.DAL.Models;
using GymManagement.DAL.Repositorities.Interfaces;
using GymManagement.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.Repositorities.Classes
{
    public class MembershipRepository : GenericRepository<Membership>, IMembershipRepository
    {
        private readonly GymDbContext _context;

        public MembershipRepository(GymDbContext dbcontext) : base(dbcontext)
        {
            _context = dbcontext;
        }

        public async Task<IEnumerable<Membership>> GetAllMembershipsWithMemberAndPlanAsync(CancellationToken ct)
        {
            var memberships = _context.Memberships
                .AsNoTracking()
                .Include(m => m.Member)
                .Include(m => m.Plan);
            return await memberships.ToListAsync(ct);
        }
    }
}
