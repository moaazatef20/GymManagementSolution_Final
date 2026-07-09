using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Service.InterFaces;
using GymManagement.BLL.ViewModels.SessionsViewModels;
using GymManagement.BLL.ViewModels.TrainersViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositorities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.BLL.Service.Classes
{
    public class SessionServices : ISessionServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct =default)
        {
            if (model.StartDate <= DateTime.Now) return Result.Validation("Session Must Be in the Future");
            else if (model.EndDate <= model.StartDate) return Result.Validation("Start Session Cant Be After End");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId,ct);
            if (trainer is null) return Result.NotFound("Trainer Not Found");

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId, ct);
            if (category is null) return Result.NotFound("Category Not Found");


            var mappedSession = _mapper.Map<CreateSessionViewModel, Session>(model);
            await _unitOfWork.GetRepository<Session>().AddAsync(mappedSession);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Result.Ok() : Result.Fail("Fail To Add Session");
        }

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct)
        {
            var sessions = await _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategoryAsync(ct);
            if (!sessions.Any()) return [];

            sessions = sessions.OrderByDescending(s => s.StartDate);
            var mappedSessions = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);
            foreach (var session in mappedSessions)
            {
                session.AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(session.Id, ct);
            }

            return mappedSessions;
        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetAllTrainersAsync(CancellationToken ct)
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(false,ct);
            if (trainers == null) return [];
            return _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);
        }
        public async Task<IEnumerable<CategorySelectViewModel>> GetAllCategorysAsync(CancellationToken ct)
        {
            var categorys = await _unitOfWork.GetRepository<Category>().GetAllAsync(false, ct);
            if (categorys == null) return [];
            return _mapper.Map<IEnumerable<CategorySelectViewModel>>(categorys);
        }

        public async Task<UpdateSessionViewModel?> GetSessionToUpdateAsync(int id, CancellationToken ct = default)
        {
            var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(id, ct);
            if (session == null) return null;
            var mappingSession = _mapper.Map<Session, UpdateSessionViewModel>(session);
            return (mappingSession);
        }

        public async Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default)
        {
            var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(id, ct);
            if (session == null) return Result.NotFound("Session Not Found");
            if (session?.StartDate <= DateTime.UtcNow && session.EndDate >= DateTime.UtcNow) return Result.Validation("Cant Edit Session OnGoing Only Upcoming Sessions");
            if (model.StartDate <= DateTime.Now) return Result.Validation("Session Must Be in the Future");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId, ct);
            if (trainer is null) return Result.NotFound("Trainer Not Found");

            var mappedSession = _mapper.Map(model,session);
            await _unitOfWork.GetRepository<Session>().UpdateAsync(mappedSession,ct);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Result.Ok() : Result.Fail("Fail To Update Session");
        }

        public async Task<Result> DeleteSessionAsync(int id, CancellationToken ct = default)
        {
            var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(id);
            if (session?.StartDate <= DateTime.Now && session.EndDate >= DateTime.Now) return Result.Validation("Cant Delete Session OnGoing Wait Until End");
            var bookedCount = await _unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(session.Id, ct);
            if (bookedCount > 0) return Result.Validation("Can't Delete a Session Has Bookings");
            await _unitOfWork.GetRepository<Session>().DeleteAsync(session, ct);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Result.Ok() : Result.Fail("Fail Delete");
        }

        public async Task<SessionViewModel?> GetSessionDetailsAsync(int id, CancellationToken ct = default)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdSessionsWithTrainerAndCategoryAsync(id, ct);

            if (session == null) return null;
            var mappingSession = _mapper.Map<Session, SessionViewModel>(session);
            mappingSession.AvailableSlots = mappingSession.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotAsync(id, ct);
            return (mappingSession);
        }
    }
}
