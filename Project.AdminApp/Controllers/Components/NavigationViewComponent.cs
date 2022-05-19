using Project.AdminApp.Models;

/*using Project.ApiIntegration;
using Project.Utilities.Constants;*/

using Project.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using Project.AdminApp.Services;
using Project.Utilities.Constants;

namespace Project.AdminApp.Controllers.Components
{
    public class NavigationViewComponent : ViewComponent
    {
        private readonly ILanguageApiClient _languageApiClient;

        public NavigationViewComponent(ILanguageApiClient languageApiClient)
        {
            _languageApiClient = languageApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var languages = await _languageApiClient.GetAll();
            var currentLanguageId = HttpContext
                .Session
                .GetString(SystemConstants.AppSettings.DefaultLanguageId);
            /*  var items = languages.ResultObj.Select(x => new SelectListItem()
              {
                  Text = x.Name,
                  Value = x.Id.ToString(),
                  Selected = currentLanguageId == null ? x.IsDefault : currentLanguageId == x.Id.ToString()
              });*/
            var navigationVm = new NavigationViewModel()
            {
                CurrentLanguageId = currentLanguageId,
                Languages = languages.ResultObj
                /*items.ToList()*/
            };

            return View("Default", navigationVm);
        }
    }
}