using Project.Application.System.Roles;
using Project.Data.EF;
using Project.Data.Entities;
using Project.ViewModel.System.Roles;
using Project.ViewModel.Utilities.Slides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Data;

namespace Project.Application.Utilities.Slides
{
    public class SlideService : ISlideService
    {
        private readonly ProjectDbContext _context;

        public SlideService(ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<List<SlideVm>> GetAll()
        {
            var slides = await _context.Slides.OrderBy(x => x.SortOrder)
                .Select(x => new SlideVm()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Url = x.Url,
                    Image = x.Image
                }).ToListAsync();

            return slides;
        }
    }
}