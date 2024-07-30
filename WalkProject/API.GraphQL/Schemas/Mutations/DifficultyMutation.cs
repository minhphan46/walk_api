using AppAny.HotChocolate.FluentValidation;
using AutoMapper;
using HotChocolate.Authorization;
using WalkProject.API.GraphQL.DTOs.Difficulties;
using WalkProject.API.GraphQL.Resolvers;
using WalkProject.DataModels.Entities;

namespace WalkProject.API.GraphQL.Schemas.Mutations
{
    [ExtendObjectType("Mutation")]
    public class DifficultyMutation
    {
        private readonly DifficultiesResolver resolver;
        private readonly IMapper mapper;

        public DifficultyMutation(DifficultiesResolver resolver, IMapper mapper)
        {
            this.resolver = resolver;
            this.mapper = mapper;
        }

        // Create Difficulty
        [Authorize]
        public async Task<DifficultyResponse> CreateDifficulty(DifficultyInput difficultyInput)
        {
            var difficulty = mapper.Map<Difficulty>(difficultyInput);

            difficulty = await resolver.CreateAsync(difficulty);

            var difficultyResponse = mapper.Map<DifficultyResponse>(difficulty);

            return difficultyResponse;
        }

        // Update Difficulty
        [Authorize]
        public async Task<DifficultyResponse> UpdateDifficulty(Guid difficultyId, [UseFluentValidation] DifficultyInput difficultyInput)
        {
            var difficultyDomain = mapper.Map<Difficulty>(difficultyInput);
            // check if the difficulty exists
            difficultyDomain = await resolver.UpdateAsync(difficultyId, difficultyDomain);

            if (difficultyDomain == null)
            {
                throw new GraphQLException(new Error("Difficulty not found.", "DIFFICULTY_NOT_FOUND"));
            }

            var difficultyResponse = mapper.Map<DifficultyResponse>(difficultyDomain);

            return difficultyResponse;
        }

        // Delete Difficulty
        [Authorize(Policy = "IsAdmin")]
        public async Task<DifficultyResponse> DeleteDifficulty(Guid difficultyId)
        {
            var difficultyDomainModel = await resolver.DeleteAsync(difficultyId);

            if (difficultyDomainModel == null)
            {
                throw new GraphQLException(new Error("Difficulty not found.", "DIFFICULTY_NOT_FOUND"));
            }

            var difficultyResponse = mapper.Map<DifficultyResponse>(difficultyDomainModel);


            return difficultyResponse;
        }
    }
}
