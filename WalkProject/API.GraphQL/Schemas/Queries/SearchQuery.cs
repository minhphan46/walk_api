using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WalkProject.API.GraphQL.DTOs.Base;
using WalkProject.API.GraphQL.DTOs.Categories;
using WalkProject.API.GraphQL.DTOs.Difficulties;
using WalkProject.API.GraphQL.DTOs.Regions;
using WalkProject.API.GraphQL.DTOs.Walks;
using WalkProject.API.GraphQL.Resolvers;
using WalkProject.DataModels.DbContexts;

namespace WalkProject.API.GraphQL.Schemas.Queries
{
    [ExtendObjectType("Query")]
    public class SearchQuery
    {
        private readonly WalksResolver _resolver;
        private readonly IMapper mapper;

        public SearchQuery(WalksResolver walksResolver, IMapper mapper)
        {
            _resolver = walksResolver;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<IBaseResponse>> Search(string text, [Service] NZWalksDbContext context)
        {
            IEnumerable<WalkResponse> walks = await context.Walks
                        .Where(w => w.Name == text)
                        .Select(w => new WalkResponse()
                        {
                            Id = w.Id,
                            Name = w.Name,
                            Description = w.Description,
                            DifficultyId = w.DifficultyId,
                            LengthInKm = w.LengthInKm,
                            RegionId = w.RegionId,
                            WalkImageUrl = w.WalkImageUrl
                        }
                   ).ToListAsync();

            IEnumerable<CategoryResponse> categories = await context.Categories
                        .Where(w => w.Name == text)
                        .Select(w => new CategoryResponse()
                        {
                            Id = w.Id,
                            Name = w.Name
                        }
                   ).ToListAsync();

            IEnumerable<DifficultyResponse> difficulties = await context.Difficulties
                        .Where(w => w.Name == text)
                        .Select(w => new DifficultyResponse()
                        {
                            Id = w.Id,
                            Name = w.Name
                        }
                   ).ToListAsync();

            IEnumerable<RegionResponse> regions = await context.Regions
                .Where(w => w.Name == text)
                        .Select(w => new RegionResponse()
                        {
                            Id = w.Id,
                            Name = w.Name,
                            Code = w.Code,
                            RegionImageUrl = w.RegionImageUrl
                        }
                   ).ToListAsync();

            return new List<IBaseResponse>()
                        .Concat(walks)
                        .Concat(categories)
                        .Concat(difficulties)
                        .Concat(regions);
        }
    }
}
