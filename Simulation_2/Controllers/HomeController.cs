using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simulation_2.DAL;
using Simulation_2.Models;
using Simulation_2.ViewModels.Home;

namespace Simulation_2.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
            
        }
        public async Task<IActionResult> Index()
        {
            List<TeamMember> members =await _context.TeamMembers.ToListAsync();

            HomeVm homeVm = new HomeVm()
            {
                TeamMembers = members,
            };
            return View(homeVm);
        }
    }
}
