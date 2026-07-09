using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Service.InterFaces;
using GymManagement.BLL.ViewModels.TrainersViewModels;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositorities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.BLL.Service.Classes
{
    public class TrainerService : ITrainerServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainers(CancellationToken ct = default)
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);
            if (!trainers.Any()) return [];
            var mappingTrainers = _mapper.Map<IEnumerable<TrainerViewModel>>(trainers);
            return (mappingTrainers);
        }

        public async Task<Result> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            var emailExist = await _unitOfWork.GetRepository<Trainer>().AnyAsync(e => e.Email == model.Email, ct);
            var phoneExist = await _unitOfWork.GetRepository<Trainer>().AnyAsync(e => e.Phone == model.Phone, ct);
            if (emailExist) return Result.Validation("Email is already exist");
            else if (phoneExist) return Result.Validation("Phone is already exist");
            var mappingTrainers = _mapper.Map<CreateTrainerViewModel,Trainer>(model);
            await _unitOfWork.GetRepository<Trainer>().AddAsync(mappingTrainers,ct);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Result.Ok() : Result.Fail("Failed to Create Trainer");
        }

        public async Task<Result> DeleteTrainerAsync(int id, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id);
            var bookings = await _unitOfWork.GetRepository<Session>().AnyAsync(b => b.TrainerId == id && b.EndDate > DateTime.UtcNow, ct);
            if (trainer == null) return Result.NotFound("Trainer Not Found");
            else if (bookings) return Result.Fail("Can't Delete Trainer Until Session Ended");
            await _unitOfWork.GetRepository<Trainer>().DeleteAsync(trainer, ct);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Result.Ok() : Result.Fail("Fail Delete");
        }
            

        public async Task<TrainerViewModel?> GetTrainerDetailsAsync(int id, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id);
            if (trainer == null) return null;
            var mappingTrainer = _mapper.Map<Trainer, TrainerViewModel>(trainer);
            return (mappingTrainer);
        }

        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int id, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id ,ct);
            if (trainer == null) return null;
            var mappingTrainer = _mapper.Map<Trainer, TrainerToUpdateViewModel>(trainer);
            return (mappingTrainer);
        }

        public async Task<Result> UpdateTrainerAsync(int id, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, ct);
            if (trainer == null) return Result.NotFound("Trainer Not Found");
            else if (await _unitOfWork.GetRepository<Trainer>().AnyAsync(e => e.Email == model.Email && e.Id != id)) return Result.Validation("Email is Already Exist");
            else if (await _unitOfWork.GetRepository<Trainer>().AnyAsync(e => e.Phone == model.Phone && e.Id != id)) return Result.Validation("Phone is Already Exist");
            var mappedTrainer = _mapper.Map(model,trainer);
            await _unitOfWork.GetRepository<Trainer>().UpdateAsync(trainer, ct);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Result.Ok() : Result.Fail("Failed to Update Trainer");
        }
    }
}

