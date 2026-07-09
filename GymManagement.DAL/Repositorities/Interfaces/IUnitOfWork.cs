using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.Repositorities.Interfaces
{
    public interface IUnitOfWork
    {
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();
        public Task<int> CompleteAsync();
        public ISessionRepository SessionRepository { get; }
        public IMembershipRepository MembershipRepository { get; }
        public IBookingRepository BookingRepository { get; }
    }
}
