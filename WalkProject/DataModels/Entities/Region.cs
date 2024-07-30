using WalkProject.DataModels.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace WalkProject.DataModels.Entities
{
    public class Region : IBaseModel
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        public string RegionImageUrl { get; set; }
    }
}
