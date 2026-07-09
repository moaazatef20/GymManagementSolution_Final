using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.Repositorities.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<IEnumerable<Booking>> GetBookingsForSessionWithMemberAsync(int sessionId, CancellationToken ct);
    }
}
