using HotChocolate.Authorization;
using WalkProject.DataModels.Entities;
using WalkProject.Middlewares.Users;

namespace WalkProject.API.GraphQL.Schemas.Queries
{
    [ExtendObjectType("Query")]
    public class UserQuery
    {
        [Authorize]
        [UseUser]
        public User GetMe([User] User user)
        {
            return user;
        }

        // Get All Users

        // Get User By Id
    }
}
