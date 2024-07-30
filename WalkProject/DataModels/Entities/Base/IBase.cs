using System.ComponentModel.DataAnnotations;

namespace WalkProject.DataModels.Entities.Base
{
    public class IBaseModel
    {
        public IBaseModel()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
