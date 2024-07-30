using AutoMapper;
using WalkProject.API.GraphQL.DTOs.Categories;
using WalkProject.API.GraphQL.Resolvers;

namespace WalkProject.API.GraphQL.Schemas.Queries
{
    [ExtendObjectType("Query")]
    public class CategoriesQuery
    {
        private readonly CategoriesResolver _resolver;
        private readonly IMapper mapper;

        public CategoriesQuery(CategoriesResolver resolver, IMapper mapper)
        {
            _resolver = resolver;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<CategoryResponse>> GetCategories()
        {
            var categories = await _resolver.GetAllAsync();
            return mapper.Map<IEnumerable<CategoryResponse>>(categories);
        }

        public async Task<CategoryResponse> GetCategory(Guid id)
        {
            var category = await _resolver.GetByIdAsync(id);

            if (category == null)
            {
                throw new GraphQLException(new Error("Category not found.", "CATEGORY_NOT_FOUND"));
            }
            return mapper.Map<CategoryResponse>(category);
        }
    }
}
