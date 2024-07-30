using HotChocolate.Data.Filters;
using WalkProject.API.GraphQL.DTOs.Walks;

namespace WalkProject.API.GraphQL.Schemas.Filters
{
    public class WalkFilterType : FilterInputType<WalkResponse>
    {
        protected override void Configure(IFilterInputTypeDescriptor<WalkResponse> descriptor)
        {
            descriptor.Ignore(w => w.WalkImageUrl);
            descriptor.Ignore(w => w.DifficultyId);
            descriptor.Ignore(w => w.RegionId);

            base.Configure(descriptor);
        }
    }
}
