using System.ComponentModel.DataAnnotations.Schema;

namespace Simulation_2.Areas.Manage.ViewModels.TeamMember
{
    public class TeamMemberVm
    {
        public string FullName { get; set; }
        public string Designation { get; set; }
        public string? ImgUrl { get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
    }
}
