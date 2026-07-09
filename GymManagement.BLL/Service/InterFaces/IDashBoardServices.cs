using GymManagement.BLL.ViewModels.DashBoardViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagement.BLL.Service.InterFaces
{
    public interface IDashBoardServices
    {
        Task<DashBoardViewModel> GetDashBoardDateAsync(CancellationToken ct=default);
    }
}
