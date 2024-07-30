using HotChocolate.Data.Sorting;
using WalkProject.API.GraphQL.DTOs.Walks;

namespace WalkProject.API.GraphQL.Schemas.Sorters
{
    public class WalkSortType : SortInputType<WalkResponse>
    {
        protected override void Configure(ISortInputTypeDescriptor<WalkResponse> descriptor)
        {
            descriptor.Ignore(c => c.Id);
            descriptor.Ignore(c => c.WalkImageUrl);
            descriptor.Ignore(w => w.DifficultyId);
            descriptor.Ignore(w => w.RegionId);

            base.Configure(descriptor);
        }
    }
}
