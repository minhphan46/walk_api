using WalkProject.DataModels.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace WalkProject.DataModels.Entities
{
    public class Role : IBaseModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
