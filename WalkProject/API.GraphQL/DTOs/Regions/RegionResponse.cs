using WalkProject.API.GraphQL.DTOs.Base;

namespace WalkProject.API.GraphQL.DTOs.Regions
{
    public class RegionResponse : IBaseResponse
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string RegionImageUrl { get; set; }
    }
}
