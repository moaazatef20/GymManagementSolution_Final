using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.Repositorities.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken ct);
        Task<Session> GetByIdSessionsWithTrainerAndCategoryAsync(int id, CancellationToken ct);
        Task<int> GetCountOfBookedSlotAsync(int sessionId, CancellationToken ct);
    }
}
