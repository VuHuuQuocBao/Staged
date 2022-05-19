using Project.ViewModel.Catalog.Categories;
using Project.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.AdminApp.Services
{
    public interface ICategoryApiClient
    {
        Task<ApiResult<List<CategoryVm>>> GetAll(string languageId);

        Task<CategoryVm> GetById(string languageId, int id);
    }
}