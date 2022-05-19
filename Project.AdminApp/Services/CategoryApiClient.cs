/*using Project.ApiIntegration;*/

using Project.ViewModel.Catalog.Categories;
using Project.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Project.AdminApp.Services;

namespace Project.AdminApp.Services
{
    public class CategoryApiClient : BaseApiClient, ICategoryApiClient
    {
        public CategoryApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<ApiResult<List<CategoryVm>>> GetAll(string languageId)
        {
            return await base.GetAsync<ApiResult<List<CategoryVm>>>("/api/categories?languageId=" + languageId);
        }

        public async Task<CategoryVm> GetById(string languageId, int id)
        {
            return await base.GetAsync<CategoryVm>($"/api/categories/{id}/{languageId}");
        }
    }
}