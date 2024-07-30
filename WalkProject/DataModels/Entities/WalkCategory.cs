using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WalkProject.DataModels.Entities
{
    public class WalkCategory
    {
        [Required]
        public Guid WalkId { get; set; }
        [Required]
        public Guid CategoryId { get; set; }

        // Navigation properties
        [ForeignKey("WalkId")]
        public Walk Walk { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
