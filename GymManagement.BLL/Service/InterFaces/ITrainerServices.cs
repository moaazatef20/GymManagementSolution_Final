using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.MembersVIewModels;
using GymManagement.BLL.ViewModels.TrainersViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.BLL.Service.InterFaces
{
    public interface ITrainerServices
    {
        Task<IEnumerable<TrainerViewModel>> GetAllTrainers(CancellationToken ct = default);

        Task<TrainerViewModel?> GetTrainerDetailsAsync(int id, CancellationToken ct = default);

        Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int id, CancellationToken ct = default);

        Task<Result> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default);

        Task<Result> UpdateTrainerAsync(int id, TrainerToUpdateViewModel model, CancellationToken ct = default);

        Task<Result> DeleteTrainerAsync(int id, CancellationToken ct = default);
    }
}
