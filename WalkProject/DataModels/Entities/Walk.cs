using WalkProject.DataModels.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WalkProject.DataModels.Entities
{
    public class Walk : IBaseModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string WalkImageUrl { get; set; }

        [Required]
        public Guid DifficultyId { get; set; }

        [Required]
        public Guid RegionId { get; set; }

        // Navigation properties
        [ForeignKey("DifficultyId")]
        public Difficulty Difficulty { get; set; }
        [ForeignKey("RegionId")]
        public Region Region { get; set; }
        public ICollection<WalkCategory> WalkCategories { get; set; }
    }
}
