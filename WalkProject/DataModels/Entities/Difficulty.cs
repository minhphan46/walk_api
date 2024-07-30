using WalkProject.DataModels.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace WalkProject.DataModels.Entities
{
    public class Difficulty : IBaseModel
    {
        [Required]
        public string Name { get; set; }
    }
}
