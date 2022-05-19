using Project.ViewModel.Common;
using Project.ViewModel.System.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.System.Users
{
    public interface IUserService
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);

        Task<ApiResult<bool>> Register(RegisterRequest request);

        Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request);

        Task<ApiResult<PagedResult<UserVm>>> GetUsersPaging(GetUserPagingRequest request);

        Task<ApiResult<UserVm>> GetById(Guid id);

        Task<ApiResult<bool>> Delete(Guid id);

        Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request);

        Task<ApiResult<RoleAssignRequest>> GetAllRoleOfUser(Guid id);

        /*
                Task<string> Authenticate(LoginRequest request);

                Task<bool> Register(RegisterRequest request);

                Task<PagedResult<UserVm>> GetUsersPaging(GetUserPagingRequest request);*/
    }
}