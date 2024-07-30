using AppAny.HotChocolate.FluentValidation;
using AutoMapper;
using HotChocolate.Authorization;
using WalkProject.API.GraphQL.DTOs.Walks;
using WalkProject.API.GraphQL.Resolvers;
using WalkProject.DataModels.Entities;

namespace WalkProject.API.GraphQL.Schemas.Mutations
{
    [ExtendObjectType("Mutation")]
    public class WalkMutation
    {
        private readonly WalksResolver _resolver;
        private readonly IMapper mapper;

        public WalkMutation(WalksResolver resolver, IMapper mapper)
        {
            _resolver = resolver;
            this.mapper = mapper;
        }

        [Authorize]
        public async Task<WalkResponse> CreateWalk(WalkInput walkInput)
        {
            var walkDomain = mapper.Map<Walk>(walkInput);

            walkDomain = await _resolver.CreateAsync(walkDomain, walkInput.CategoryId);

            var walkResponse = mapper.Map<WalkResponse>(walkDomain);

            return walkResponse;
        }

        [Authorize]
        public async Task<WalkResponse> UpdateWalk(Guid walkId, [UseFluentValidation] WalkInput walkInput)
        {
            var walkDomain = mapper.Map<Walk>(walkInput);
            // check if the walk exists
            walkDomain = await _resolver.UpdateAsync(walkId, walkDomain);

            if (walkDomain == null)
            {
                throw new GraphQLException(new Error("Walk not found.", "WALK_NOT_FOUND"));
            }

            var walkResponse = mapper.Map<WalkResponse>(walkDomain);

            return walkResponse;
        }

        [Authorize(Policy = "IsAdmin")]
        public async Task<WalkResponse> DeleteWalk(Guid walkId)
        {
            var walkDomainModel = await _resolver.DeleteAsync(walkId);

            if (walkDomainModel == null)
            {
                throw new GraphQLException(new Error("Walk not found.", "WALK_NOT_FOUND"));
            }

            var walkResponse = mapper.Map<WalkResponse>(walkDomainModel);


            return walkResponse;
        }
    }
}
