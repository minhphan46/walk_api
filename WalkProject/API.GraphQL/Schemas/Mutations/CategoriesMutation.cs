using AppAny.HotChocolate.FluentValidation;
using AutoMapper;
using HotChocolate.Authorization;
using HotChocolate.Subscriptions;
using WalkProject.API.GraphQL.DTOs.Categories;
using WalkProject.API.GraphQL.Resolvers;
using WalkProject.API.GraphQL.Schemas.Subscriptions;
using WalkProject.API.GraphQL.Validators;
using WalkProject.DataModels.Entities;

namespace WalkProject.API.GraphQL.Schemas.Mutations
{
    [ExtendObjectType("Mutation")]
    public class CategoriesMutation
    {

        private readonly CategoriesResolver _resolver;
        private readonly IMapper mapper;

        public CategoriesMutation(CategoriesResolver resolver, IMapper mapper)
        {
            _resolver = resolver;
            this.mapper = mapper;
        }

        [Authorize]
        public async Task<CategoryResponse> CreateCategory([UseFluentValidation, UseValidator<CategoryInputValidator>] CategoryInput categoryInput)
        {
            var categoryDomain = mapper.Map<Category>(categoryInput);

            categoryDomain = await _resolver.CreateAsync(categoryDomain);

            var categoryResponse = mapper.Map<CategoryResponse>(categoryDomain);

            return categoryResponse;
        }

        [Authorize]
        public async Task<CategoryResponse> UpdateCategory(Guid categoryId, [UseFluentValidation] CategoryInput categoryInput)
        {
            var categoryDomain = mapper.Map<Category>(categoryInput);
            // check if the category exists
            categoryDomain = await _resolver.UpdateAsync(categoryId, categoryDomain);

            if (categoryDomain == null)
            {
                throw new GraphQLException(new Error("Category not found.", "CATEGORY_NOT_FOUND"));
            }

            var categoryResponse = mapper.Map<CategoryResponse>(categoryDomain);

            return categoryResponse;
        }

        [Authorize(Policy = "IsAdmin")]
        public async Task<CategoryResponse> DeleteCategory(Guid categoryId)
        {
            var categoryDomainModel = await _resolver.DeleteAsync(categoryId);

            if (categoryDomainModel == null)
            {
                throw new GraphQLException(new Error("Category not found.", "CATEGORY_NOT_FOUND"));
            }

            var categoryResponse = mapper.Map<CategoryResponse>(categoryDomainModel);


            return categoryResponse;
        }

        [Authorize]
        public async Task<CategoryResponse> CreateCategorySubscription([UseFluentValidation] CategoryInput categoryInput, [Service] ITopicEventSender eventSender)
        {
            var categoryDomain = mapper.Map<Category>(categoryInput);

            categoryDomain = await _resolver.CreateAsync(categoryDomain);

            var categoryResponse = mapper.Map<CategoryResponse>(categoryDomain);

            await eventSender.SendAsync(nameof(CategoriesSubscription.CategoryCreated), categoryResponse);

            return categoryResponse;
        }

        [Authorize]
        public async Task<CategoryResponse> UpdateCategorySubscription(Guid categoryId, [UseFluentValidation] CategoryInput categoryInput, [Service] ITopicEventSender eventSender)
        {
            var categoryDomain = mapper.Map<Category>(categoryInput);
            // check if the category exists
            categoryDomain = await _resolver.UpdateAsync(categoryId, categoryDomain);

            if (categoryDomain == null)
            {
                throw new GraphQLException(new Error("Category not found.", "CATEGORY_NOT_FOUND"));
            }

            var categoryResponse = mapper.Map<CategoryResponse>(categoryDomain);

            string updateCategoryTopic = $"{categoryResponse.Id}_{nameof(CategoriesSubscription.CategoryUpdated)}";
            await eventSender.SendAsync(updateCategoryTopic, categoryResponse);

            return categoryResponse;
        }
    }
}
