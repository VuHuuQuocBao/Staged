using Project.ViewModel.Common;
using Project.ViewModel.System.Users;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Project.ViewModel;

namespace Project.AdminApp.Services
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserApiClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)

        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/users/authenticate", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiResult<string>>(result);
            }

            return JsonConvert.DeserializeObject<ApiResult<string>>(result);
        }

        public async Task<ApiResult<bool>> Delete(Guid id)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            /*var response = await client.GetAsync($"api/users/paging?pageIndex=" + $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");*/
            var response = await client.DeleteAsync($"/api/users/{id}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }

            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result); ;
        }

        public async Task<ApiResult<RoleAssignRequest>> GetAllRoleOfUser(Guid id)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            /*var response = await client.GetAsync($"api/users/paging?pageIndex=" + $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");*/
            var response = await client.GetAsync($"/api/users/getAllRoleOfUser/{id}");
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<RoleAssignRequest>>(result);
            }

            return JsonConvert.DeserializeObject<ApiErrorResult<RoleAssignRequest>>(result);
        }

        public async Task<ApiResult<UserVm>> GetById(Guid id)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            /*var response = await client.GetAsync($"api/users/paging?pageIndex=" + $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");*/
            var response = await client.GetAsync($"/api/users/{id}");

            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<UserVm>>(result);
            }

            return JsonConvert.DeserializeObject<ApiErrorResult<UserVm>>(result); ;
        }

        public async Task<ApiResult<PagedResult<UserVm>>> GetUsersPaging(GetUserPagingRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            /*var response = await client.GetAsync($"api/users/paging?pageIndex=" + $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");*/
            var response = await client.GetAsync($"/api/users/paging?pageIndex=" +
               $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");

            var result = await response.Content.ReadAsStringAsync();
            var finalResult = JsonConvert.DeserializeObject<ApiSuccessResult<PagedResult<UserVm>>>(result);
            return finalResult;
        }

        public async Task<ApiResult<bool>> RegisterUser(RegisterRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            /*var response = await client.GetAsync($"api/users/paging?pageIndex=" + $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");*/
            var response = await client.PostAsync($"/api/users", httpContent);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            /*var response = await client.GetAsync($"api/users/paging?pageIndex=" + $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");*/
            var response = await client.PutAsync($"/api/users/{id}/roles", httpContent);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest request)
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri(_configuration["BaseAddress"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", session);

            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            /*var response = await client.GetAsync($"api/users/paging?pageIndex=" + $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");*/
            var response = await client.PutAsync($"/api/users/{id}", httpContent);
            var result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }

        public async Task<bool> UploadImage(IFormFileViewModel request)
        {
            /*request.image.CopyTo(ms);
            var fileBytes = ms.ToArray();
            string s = Convert.ToBase64String(fileBytes);
            // act on the Base64 data
            var something = new IFormFileStringViewModel()
            {
                image = s
            };*/
            var client = _httpClientFactory.CreateClient();
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            var requestContent = new MultipartFormDataContent();

            if (request.image != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.image.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.image.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "image", request.image.FileName);
            }
            client.DefaultRequestHeaders.Add("Authorization", "Client-ID" + "333e27cda788d73");
            //requestContent.Add(new StringContent(request.Id.ToString()), "id");
            var response = await client.PostAsync($"https://orion-http.gw.postman.co/v1/request", requestContent);

            /*
                            var response = await client.PutAsync($"/api/products/" + request.Id, requestContent);
                            return response.IsSuccessStatusCode;*/

            /*var client = _httpClientFactory.CreateClient();*/
            /*    client.BaseAddress = new Uri(_configuration["BaseAddress"]);*/
            /*  client.DefaultRequestHeaders.Add("Authorization", "Client-ID" + "333e27cda788d73");
              var json = JsonConvert.SerializeObject(something);
              var httpContent = new StringContent(json, Encoding.UTF8, "/form-multipartdata");
              *//*var response = await client.GetAsync($"api/users/paging?pageIndex=" + $"{request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");*//*
              var response = await client.PostAsync($"https://orion-http.gw.postman.co/v1/request", httpContent);
              var result = await response.Content.ReadAsStringAsync();*/

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
}