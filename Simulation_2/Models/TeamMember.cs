using Simulation_2.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simulation_2.Models
{
    public class TeamMember:BaseEntity
    {
        public string FullName { get; set; }
        public string Designation { get; set; }
        public string? ImgUrl { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }
    }
}
