using System.ComponentModel.DataAnnotations;

namespace WalkProject.API.RestFul.DTOs.DifficultyModel
{
    public class AddDifficultyDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name has to be a maximum of 100 characters")]
        public string Name { get; set; }
    }
}
