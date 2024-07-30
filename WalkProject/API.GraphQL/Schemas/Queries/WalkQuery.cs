using AutoMapper;
using WalkProject.API.GraphQL.DTOs.Walks;
using WalkProject.API.GraphQL.Resolvers;
using WalkProject.API.GraphQL.Schemas.Filters;
using WalkProject.API.GraphQL.Schemas.Sorters;
using WalkProject.DataModels.DbContexts;

namespace WalkProject.API.GraphQL.Schemas.Queries
{
    [ExtendObjectType("Query")]
    public class WalkQuery
    {
        private readonly WalksResolver _resolver;
        private readonly IMapper mapper;

        public WalkQuery(WalksResolver walksResolver, IMapper mapper)
        {
            _resolver = walksResolver;
            this.mapper = mapper;
        }

        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 10)]
        [UseFiltering(typeof(WalkFilterType))]
        [UseSorting(typeof(WalkSortType))]
        public async Task<IEnumerable<WalkResponse>> GetWalks()
        {
            var walks = await _resolver.GetAllAsync();
            return mapper.Map<IEnumerable<WalkResponse>>(walks);
        }

        [UsePaging(IncludeTotalCount = true, DefaultPageSize = 10)]
        [UseFiltering(typeof(WalkFilterType))]
        [UseSorting(typeof(WalkSortType))]
        public IQueryable<WalkResponse> GetWalksDb([Service] NZWalksDbContext context)
        {
            return context.Walks.Select(
                        w => new WalkResponse()
                        {
                            Id = w.Id,
                            Name = w.Name,
                            Description = w.Description,
                            DifficultyId = w.DifficultyId,
                            LengthInKm = w.LengthInKm,
                            RegionId = w.RegionId,
                            WalkImageUrl = w.WalkImageUrl
                        }
                    );
        }

        public async Task<WalkResponse> GetWalk(Guid id)
        {
            var walk = await _resolver.GetByIdAsync(id);

            if (walk == null)
            {
                throw new GraphQLException(new Error("Walk not found.", "WALK_NOT_FOUND"));
            }
            return mapper.Map<WalkResponse>(walk);
        }
    }
}
