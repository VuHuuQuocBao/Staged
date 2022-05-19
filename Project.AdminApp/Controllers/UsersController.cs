using Project.AdminApp.Services;
using Project.ViewModel.System.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Project.ViewModel.Common;
using Project.Utilities;
using Microsoft.AspNetCore.Authorization;
using Project.Utilities.Constants;

namespace Project.AdminApp.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        private readonly IRoleApiClient _roleApiClient;

        public UsersController(IUserApiClient userApiClient, IConfiguration configuration, IRoleApiClient roleApiClient)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _roleApiClient = roleApiClient;
        }

        [HttpGet("cailon/githe/alo/{pageSize?}/{pageIndex?}/{keyword?}")]
        public async Task<IActionResult> Index(string keyword, int pageSize = 1, int pageIndex = 1)
        {
            var session = HttpContext.Session.GetString("Token");
            var request = new GetUserPagingRequest()
            {
                Keyword = keyword,
                PageSize = pageSize,
                PageIndex = pageIndex
            };
            var data = await _userApiClient.GetUsersPaging(request);
            ViewBag.Keyword = keyword;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data.ResultObj);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.RegisterUser(request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "success";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _userApiClient.GetById(id);

            if (result.IsSuccessed)
            {
                var user = result.ResultObj;
                var updateRequest = new UserUpdateRequest()
                {
                    Dob = user.Dob,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Id = id
                };
                return View(updateRequest);
                /* // return view và có sẵn dữ liệu chi don gian la vay, bình thường chỉ có view thôi se khong in du lieu ra input;*/
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.UpdateUser(request.Id, request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "success";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        // le ra url phai la ?id=abcxyz nhung no an di ??

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            return View(new UserDeleteRequest()
            {
                Id = id
            });
            /*return View();*/
        }

        // ke ca view ko return model ve' no' van bind duoc do tren url
        [HttpPost]
        public async Task<IActionResult> Delete(UserDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _userApiClient.Delete(request.Id);
            if (result.IsSuccessed)
            {
                TempData["result"] = "success";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()

        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _userApiClient.Authenticate(request);
            if (result.ResultObj == null)
            {
                ModelState.AddModelError("", result.Message);
                return View();
            }
            var userPrincipal = this.ValidateToken(result.ResultObj);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                IsPersistent = true,
            };
            HttpContext.Session.SetString(SystemConstants.AppSettings.DefaultLanguageId, _configuration["DefaultlanguageId"]);
            HttpContext.Session.SetString(SystemConstants.AppSettings.Token, result.ResultObj);

            await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(userPrincipal),
            authProperties);

            return RedirectToAction("Index", "Homes");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Login", "Users");
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            // detail thoi nen khong giong edit ko can bind ra input
            var result = await _userApiClient.GetById(id);
            return View(result.ResultObj);
        }

        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }

        [HttpGet]
        public async Task<IActionResult> RoleAssign(Guid id)
        {
            var result = await _userApiClient.GetAllRoleOfUser(id);
            return View(result.ResultObj);
        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign(Guid id, RoleAssignRequest request)
        {
            var result = await _userApiClient.RoleAssign(id, request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Cập nhật quyền thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            var roleAssignRequest = await _userApiClient.GetAllRoleOfUser(id);

            return View(roleAssignRequest.ResultObj);
        }

        /*        [HttpGet]
                public async Task<IActionResult> RoleAssign(Guid id)
                {
                    var roleAssignRequest = await GetRoleAssignRequest(id);
                    return View(roleAssignRequest.);
                }

                [HttpPost]
                public async Task<IActionResult> RoleAssign(RoleAssignRequest request)
                {
                    if (!ModelState.IsValid)
                        return View();

                    var result = await _userApiClient.RoleAssign(request.Id, request);

                    if (result.IsSuccessed)
                    {
                        TempData["result"] = "Cập nhật quyền thành công";
                        return RedirectToAction("Index");
                    }

                    ModelState.AddModelError("", result.Message);
                    var roleAssignRequest = await GetRoleAssignRequest(request.Id);

                    return View(roleAssignRequest);
                }

                private async Task<RoleAssignRequest> GetRoleAssignRequest(Guid id)
                {
                    var userObj = await _userApiClient.GetById(id);
                    var roleObj = await _roleApiClient.GetAll();
                    var roleAssignRequest = new RoleAssignRequest();
                    foreach (var role in roleObj.ResultObj)
                    {
                        roleAssignRequest.Roles.Add(new SelectItem()
                        {
                            Id = role.Id.ToString(),
                            Name = role.Name,
                            Selected = userObj.ResultObj.Roles.Contains(role.Name).ToString() + "123"
                        });
                    }
                    return roleAssignRequest;
                }*/
    }
}