using Microsoft.AspNetCore.Mvc;
using Project.ViewModel.Common;
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
using Microsoft.AspNetCore.Mvc.Rendering;
using Project.ViewModel;

namespace Project.AdminApp.Controllers
{
    public class TestController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        private readonly IRoleApiClient _roleApiClient;

        public TestController(IUserApiClient userApiClient, IConfiguration configuration, IRoleApiClient roleApiClient)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _roleApiClient = roleApiClient;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<SelectListItem> Countries = new List<SelectListItem> {
            new SelectListItem { Value = "MX", Text = "Mexico",Selected =false },
            new SelectListItem { Value = "CA", Text = "Canada",Selected =false },
            new SelectListItem { Value = "US", Text = "USA" ,Selected =true }
            };
            ViewBag.Countries = Countries;
            /*ViewBag.selected = "CA";*/
            var model = new Class1();
            /*model.Country = "CA";*/
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(/*Class1 model*/[FromForm] string abc, string bcd)
        {
            if (ModelState.IsValid)
            {
                /*  var msg = model.Country + " selected";*/
                /*                return RedirectToAction("IndexSuccess", new { message = msg });*/
            }
            var a = abc;

            // If we got this far, something failed; redisplay form.
            return View(/*model*/);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create1([FromForm] IFormFileViewModel request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _userApiClient.UploadImage(request);
            if (result)
            {
                TempData["result"] = "Thêm mới sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Thêm sản phẩm thất bại");
            return View(request);
        }
    }
}