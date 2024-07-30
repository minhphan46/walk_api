using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using WalkProject.API.GraphQL.DTOs.Categories;

namespace WalkProject.API.GraphQL.Schemas.Subscriptions
{
    [ExtendObjectType("Subscription")]
    public class CategoriesSubscription
    {
        [Subscribe]
        public CategoryResponse CategoryCreated([EventMessage] CategoryResponse category) => category;

        public ValueTask<ISourceStream<CategoryResponse>> SubscribeToCategoryUpdated(Guid categoryId, [Service] ITopicEventReceiver receiver)
        {
            string topicName = $"{categoryId}_{nameof(CategoriesSubscription.CategoryUpdated)}";

            return receiver.SubscribeAsync<CategoryResponse>(topicName);
        }

        [Subscribe(With = nameof(SubscribeToCategoryUpdated))]
        public CategoryResponse CategoryUpdated([EventMessage] CategoryResponse category) => category;
    }
}
