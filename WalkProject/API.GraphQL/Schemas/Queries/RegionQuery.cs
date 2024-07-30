using AutoMapper;
using WalkProject.API.GraphQL.DTOs.Regions;
using WalkProject.API.GraphQL.Resolvers;

namespace WalkProject.API.GraphQL.Schemas.Queries
{
    [ExtendObjectType("Query")]
    public class RegionQuery
    {
        private readonly RegionsResolver _resolver;
        private readonly IMapper mapper;

        public RegionQuery(RegionsResolver regionsResolver, IMapper mapper)
        {
            _resolver = regionsResolver;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<RegionResponse>> GetRegions()
        {
            var regions = await _resolver.GetAllAsync();
            return mapper.Map<IEnumerable<RegionResponse>>(regions);
        }

        public async Task<RegionResponse> GetRegion(Guid id)
        {
            var region = await _resolver.GetByIdAsync(id);

            if (region == null)
            {
                throw new GraphQLException(new Error("Region not found.", "REGION_NOT_FOUND"));
            }
            return mapper.Map<RegionResponse>(region);
        }
    }
}
