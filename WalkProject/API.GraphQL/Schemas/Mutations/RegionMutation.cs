using AppAny.HotChocolate.FluentValidation;
using AutoMapper;
using HotChocolate.Authorization;
using WalkProject.API.GraphQL.DTOs.Regions;
using WalkProject.API.GraphQL.Resolvers;
using WalkProject.DataModels.Entities;

namespace WalkProject.API.GraphQL.Schemas.Mutations
{
    [ExtendObjectType("Mutation")]
    public class RegionMutation
    {
        private readonly RegionsResolver _resolver;
        private readonly IMapper mapper;

        public RegionMutation(RegionsResolver resolver, IMapper mapper)
        {
            _resolver = resolver;
            this.mapper = mapper;
        }

        [Authorize]
        public async Task<RegionResponse> CreateRegion(RegionInput regionInput)
        {
            var regionDomain = mapper.Map<Region>(regionInput);

            regionDomain = await _resolver.CreateAsync(regionDomain);

            var regionResponse = mapper.Map<RegionResponse>(regionDomain);

            return regionResponse;
        }

        [Authorize]
        public async Task<RegionResponse> UpdateRegion(Guid regionId, [UseFluentValidation] RegionInput regionInput)
        {
            var regionDomain = mapper.Map<Region>(regionInput);
            // check if the region exists
            regionDomain = await _resolver.UpdateAsync(regionId, regionDomain);

            if (regionDomain == null)
            {
                throw new GraphQLException(new Error("Region not found.", "REGION_NOT_FOUND"));
            }

            var regionResponse = mapper.Map<RegionResponse>(regionDomain);

            return regionResponse;
        }

        [Authorize(Policy = "IsAdmin")]
        public async Task<RegionResponse> DeleteRegion(Guid regionId)
        {
            var regionDomainModel = await _resolver.DeleteAsync(regionId);

            if (regionDomainModel == null)
            {
                throw new GraphQLException(new Error("Region not found.", "REGION_NOT_FOUND"));
            }

            var regionResponse = mapper.Map<RegionResponse>(regionDomainModel);


            return regionResponse;
        }
    }
}
