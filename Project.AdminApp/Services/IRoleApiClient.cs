using Project.ViewModel.Common;
using Project.ViewModel.System.Roles;

namespace Project.AdminApp.Services
{
    public interface IRoleApiClient
    {
        Task<ApiResult<List<RoleVm>>> GetAll();
    }
}