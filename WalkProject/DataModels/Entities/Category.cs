using WalkProject.DataModels.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace WalkProject.DataModels.Entities
{
    public class Category : IBaseModel
    {
        [Required]
        public string Name { get; set; }

        // Navigation property
        public ICollection<WalkCategory> WalkCategories { get; set; }
    }
}
