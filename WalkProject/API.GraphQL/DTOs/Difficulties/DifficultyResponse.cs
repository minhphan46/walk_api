using WalkProject.API.GraphQL.DTOs.Base;

namespace WalkProject.API.GraphQL.DTOs.Difficulties
{
    public class DifficultyResponse : IBaseResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
