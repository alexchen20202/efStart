using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using efStart3.Models;
using efStart3.Models.SchoolViewModels;
using efStart3.DAL;
using Microsoft.EntityFrameworkCore;

namespace efStart3.Controllers
{

    public class HomeController : Controller
    {
        // private readonly ILogger<HomeController> _logger;
        
        // public HomeController(ILogger<HomeController> logger)
        // {
        //     _logger = logger;            
        // }

        private readonly SchoolContext _context;
        public HomeController(SchoolContext context)
        {
            _context = context;            
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {
            IQueryable<EnrollmentDateGroup> eGroups = 
            from student in _context.Students
                group student by student.EnrollmentDate into sGroup
                select new EnrollmentDateGroup()
                {
                    EnrollmentDate = sGroup.Key,
                    StudentCount = sGroup.Count(),
                };
            return View(await eGroups.AsNoTracking().ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
