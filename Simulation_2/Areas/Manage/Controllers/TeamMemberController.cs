using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simulation_2.Areas.Manage.ViewModels.TeamMember;
using Simulation_2.DAL;
using Simulation_2.Helpers.Extentions;
using Simulation_2.Models;
using System.Numerics;

namespace Simulation_2.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin")]
    public class TeamMemberController : Controller
    {
        AppDbContext _context;
        IWebHostEnvironment _env;

        public TeamMemberController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }   

        public async Task<IActionResult> Index()
        {
            List<TeamMember> members = await _context.TeamMembers.ToListAsync();

            return View(members);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TeamMemberVm teamMemberVm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!teamMemberVm.File.ContentType.Contains("image"))
            {
                ModelState.AddModelError("File", "Duzgun format daxil edin");
                return View();
            }

            if (teamMemberVm.File.Length >= 2097152)
            {
                ModelState.AddModelError("File", "File is too long");
                return View();
            }

            teamMemberVm.ImgUrl = teamMemberVm.File.CreateFile(_env.WebRootPath, "Upload/TeamMembers");

            TeamMember member = new TeamMember()
            {
                FullName = teamMemberVm.FullName,
                Designation = teamMemberVm.Designation,
                ImgUrl = teamMemberVm.ImgUrl,
                File = teamMemberVm.File
            };

            await _context.TeamMembers.AddAsync(member);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var member = await _context.TeamMembers.FirstOrDefaultAsync(x => x.Id == id);

            if (member == null)
            {
                return NotFound();
            }

            member.ImgUrl.RemoveFile(_env.WebRootPath, "Upload/TeamMembers");

            _context.TeamMembers.Remove(member);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public IActionResult Update(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var dbMember = _context.TeamMembers.FirstOrDefault(x => x.Id == id);
            if (dbMember == null)
            {
                return NotFound();
            }

            TeamMemberVm teamMemberVm = new TeamMemberVm()
            {
                FullName = dbMember.FullName,
                Designation = dbMember.Designation,
                ImgUrl = dbMember.ImgUrl,
                File = dbMember.File
            };

            return View(teamMemberVm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id, TeamMemberVm teamMemberVm)
        {
            if (id == null)
            {
                return BadRequest();
            }

            TeamMember dbTeamMember = await _context.TeamMembers.FirstOrDefaultAsync(x => x.Id == id);

            if (dbTeamMember == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (teamMemberVm.File != null)
            {
                if (!teamMemberVm.File.ContentType.Contains("image"))
                {
                    ModelState.AddModelError("File", "Duzgun format daxil edin");
                    return View();
                }

                if (teamMemberVm.File.Length >= 2097152)
                {
                    ModelState.AddModelError("File", "File is too long");
                    return View();
                }

                dbTeamMember.ImgUrl.RemoveFile(_env.WebRootPath, "/Upload/Doctor");
                dbTeamMember.ImgUrl = teamMemberVm.File.CreateFile(_env.WebRootPath, "Upload/Doctor");
            }



            dbTeamMember.FullName = teamMemberVm.FullName;
            dbTeamMember.Designation = teamMemberVm.Designation;


            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
