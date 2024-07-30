using WalkProject.API.GraphQL.DTOs.Base;

namespace WalkProject.API.GraphQL.DTOs.Categories
{
    public class CategoryResponse : IBaseResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
