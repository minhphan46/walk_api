using WalkProject.API.RestFul.DTOs.DifficultyModel;
using WalkProject.API.RestFul.DTOs.RegionModel;
using WalkProject.API.RestFul.DTOs.WalkCategoryModel;

namespace WalkProject.API.RestFul.DTOs.WalkModel
{
    public class WalkDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string WalkImageUrl { get; set; }

        public RegionDto Region { get; set; }
        public DifficultyDto Difficulty { get; set; }

        public ICollection<WalkCategoryDto> Categories { get; set; }
    }
}
