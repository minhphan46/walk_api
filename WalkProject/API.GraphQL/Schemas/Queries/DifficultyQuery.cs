using AutoMapper;
using WalkProject.API.GraphQL.DTOs.Difficulties;
using WalkProject.API.GraphQL.Resolvers;

namespace WalkProject.API.GraphQL.Schemas.Queries
{
    [ExtendObjectType("Query")]
    public class DifficultyQuery
    {
        private readonly DifficultiesResolver _resolver;
        private readonly IMapper mapper;

        public DifficultyQuery(DifficultiesResolver difficultiesResolver, IMapper mapper)
        {
            _resolver = difficultiesResolver;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<DifficultyResponse>> GetDifficulties()
        {
            var difficulties = await _resolver.GetAllAsync();
            return mapper.Map<IEnumerable<DifficultyResponse>>(difficulties);
        }

        public async Task<DifficultyResponse> GetDifficulty(Guid id)
        {
            var difficulty = await _resolver.GetByIdAsync(id);

            if (difficulty == null)
            {
                throw new GraphQLException(new Error("Difficulty not found.", "DIFFICULTY_NOT_FOUND"));
            }
            return mapper.Map<DifficultyResponse>(difficulty);
        }
    }
}
