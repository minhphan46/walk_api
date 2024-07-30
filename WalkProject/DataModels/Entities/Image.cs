using WalkProject.DataModels.Entities.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WalkProject.DataModels.Entities
{
    public class Image : IBaseModel
    {
        [NotMapped] // Database ko chua file
        [Required]
        public IFormFile File { get; set; }

        public string FileName { get; set; }
        public string FileDescription { get; set; }
        public string FileExtension { get; set; }
        public long FileSizeInBytes { get; set; }
        public string FilePath { get; set; }
    }
}
