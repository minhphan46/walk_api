using HotChocolate.Authorization;
using WalkProject.API.GraphQL.Resolvers;
using WalkProject.DataModels.Entities;
using WalkProject.Middlewares.Users;

namespace WalkProject.API.GraphQL.Schemas.Queries
{
    [ExtendObjectType("Query")]
    public class UserQuery
    {
        private readonly UsersResolver resolver;

        public UserQuery(UsersResolver resolver)
        {
            this.resolver = resolver;
        }

        [Authorize]
        [UseUser]
        public async Task<User> GetMe([User] User user)
        {
            user = await resolver.GetMe(user.IdentityId);
            return user;
        }

        // Get All Role
        [Authorize(Policy = "IsAdmin")]
        public async Task<List<Role>> GetRoles()
        {
            return await resolver.GetAllRolesAsync();
        }

        // Get All Users
        [Authorize(Policy = "IsAdmin")]
        public async Task<List<User>> GetUsers()
        {
            return await resolver.GetAllAsync();
        }


        // Get User By Id
        [Authorize(Policy = "IsAdmin")]
        public async Task<User> GetUser(Guid id)
        {
            var user = await resolver.GetByIdAsync(id);

            if (user == null)
            {
                throw new GraphQLException(new Error("User not found.", "USER_NOT_FOUND"));
            }
            return user;
        }

        // Get User By Email
        [Authorize(Policy = "IsAdmin")]
        public async Task<User> GetUserByEmail(string email)
        {
            var user = await resolver.GetByEmailAsync(email);

            if (user == null)
            {
                throw new GraphQLException(new Error("User not found.", "USER_NOT_FOUND"));
            }
            return user;
        }
    }
}
