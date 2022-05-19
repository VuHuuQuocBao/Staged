using Project.ViewModel.Utilities.Slides;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.AdminApp.Services
{
    public interface ISlideApiClient
    {
        Task<List<SlideVm>> GetAll();
    }
}