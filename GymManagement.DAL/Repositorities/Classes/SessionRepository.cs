using GymManagement.DAL.Models;
using GymManagement.DAL.Repositorities.Interfaces;
using GymManagement.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text;

namespace GymManagement.DAL.Repositorities.Classes
{
    
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _context;

        public SessionRepository(GymDbContext dbcontext) : base(dbcontext)
        {
            _context = dbcontext;
        }
        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct)
        {
            var sessions = _context.Session
                .AsNoTracking()
                .Include(s => s.Trainer)
                .Include(s => s.Category);
            return await sessions.ToListAsync(ct);
        }

        public async Task<Session> GetByIdSessionsWithTrainerAndCategoryAsync(int id, CancellationToken ct)
        {
            var session = _context.Session
                .Include(s => s.Trainer)
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.Id == id, ct);
            return await session;
        }

        public async Task<int> GetCountOfBookedSlotAsync(int sessionId, CancellationToken ct)
        {
            return await _context.Bookings.AsNoTracking().CountAsync(s => s.SessionId == sessionId, ct);

        }
    }
}
