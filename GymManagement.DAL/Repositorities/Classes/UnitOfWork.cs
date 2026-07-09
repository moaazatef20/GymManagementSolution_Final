using GymManagement.DAL.Models;
using GymManagement.DAL.Repositorities.Interfaces;
using GymManagement.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.DAL.Repositorities.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _context;
        private readonly Dictionary<string, object> _repositories = [];
        public UnitOfWork(GymDbContext context)
        {
            _context = context;
            SessionRepository = new SessionRepository(_context);
            MembershipRepository = new MembershipRepository(_context);
            BookingRepository = new BookingRepository(_context);
        }

        public ISessionRepository SessionRepository { get; }
        public IMembershipRepository MembershipRepository { get; }
        public IBookingRepository BookingRepository { get; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var TypeName = typeof(TEntity).Name;
            if(_repositories.TryGetValue(TypeName, out object OldRepository))
                return (IGenericRepository<TEntity>)OldRepository;

            var NewRepository = new GenericRepository<TEntity>(_context);
            _repositories[TypeName] = NewRepository;
            return NewRepository;
        }
    }
}
