using GymManagement.DAL.Models;
using GymManagement.DAL.Repositorities.Interfaces;
using GymManagement.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.Repositorities.Classes
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly GymDbContext _context;

        public BookingRepository(GymDbContext dbcontext) : base(dbcontext)
        {
            _context = dbcontext;
        }

        public async Task<IEnumerable<Booking>> GetBookingsForSessionWithMemberAsync(int sessionId, CancellationToken ct)
        {
            var bookings = _context.Bookings
                .AsNoTracking()
                .Include(b => b.Member)
                .Where(b => b.SessionId == sessionId);
            return await bookings.ToListAsync(ct);
        }
    }
}
