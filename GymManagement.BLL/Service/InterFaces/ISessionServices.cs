using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.SessionsViewModels;
using GymManagement.BLL.ViewModels.TrainersViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.BLL.Service.InterFaces
{
    public interface ISessionServices
    {
        Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync(CancellationToken ct);
        Task<Result> CreateSessionAsync(CreateSessionViewModel model,CancellationToken ct = default);
        Task<IEnumerable<TrainerSelectViewModel>> GetAllTrainersAsync(CancellationToken ct);
        Task<IEnumerable<CategorySelectViewModel>> GetAllCategorysAsync(CancellationToken ct);

        Task<SessionViewModel?> GetSessionDetailsAsync(int id, CancellationToken ct = default);
        Task<UpdateSessionViewModel?> GetSessionToUpdateAsync(int id, CancellationToken ct = default);
        Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel model, CancellationToken ct = default);
        Task<Result> DeleteSessionAsync(int id, CancellationToken ct = default);
    }
}
