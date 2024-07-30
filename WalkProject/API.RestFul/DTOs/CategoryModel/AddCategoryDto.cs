using System.ComponentModel.DataAnnotations;

namespace WalkProject.API.RestFul.DTOs.CategoryModel
{
    public class AddCategoryDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name has to be a maximum of 100 characters")]
        public string Name { get; set; }
    }
}
