using GymManagement.BLL.Service.InterFaces;
using GymManagement.BLL.ViewModels.DashBoardViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositorities.Interfaces;
using GymManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.BLL.Service.Classes
{
    public class DashBoardServices : IDashBoardServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashBoardServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<DashBoardViewModel> GetDashBoardDateAsync(CancellationToken ct = default)
        {
            var sessions = await _unitOfWork.GetRepository<Session>().GetAllAsync(ct:ct);

            var memberships = await _unitOfWork.GetRepository<Membership>().CountAsync(ct:ct);

            var totalMembers = await _unitOfWork.GetRepository<Member>().CountAsync(ct:ct);

            var totalTrainers = await _unitOfWork.GetRepository<Trainer>().CountAsync(ct:ct);

            return new DashBoardViewModel
            {
                TotalMembers = totalMembers,
                TotalTrainers = totalTrainers,
                ActiveMembers = memberships,
                UpcomingSessions = sessions.Count(x => x.StartDate > DateTime.Now),
                OngoingSessions = sessions.Count(x=> x.StartDate <= DateTime.Now && x.EndDate > DateTime.Now),
                CompletedSessions = sessions.Count(x=> x.EndDate < DateTime.Now)
            };
        }
    }
}
