using WalkProject.DataModels.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WalkProject.DataModels.Entities
{
    public class User : IBaseModel
    {
        [Required]
        public string IdentityId { get; set; }
        [Required]
        public string Email { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public DateTime? Dob { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool EmailVerified { get; set; }
        [Required]
        public Guid RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }
        public bool Status { get; set; }
    }
}
