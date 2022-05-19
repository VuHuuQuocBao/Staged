using Project.ViewModel.Catalog.Categories;
using Project.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Application.Catalog.Categories
{
    public interface ICategoryService
    {
        Task<ApiResult<List<CategoryVm>>> GetAll(string languageId);

        Task<CategoryVm> GetById(string languageId, int id);
    }
}