using AppAny.HotChocolate.FluentValidation;
using AutoMapper;
using HotChocolate.Authorization;
using WalkProject.API.GraphQL.DTOs.Categories;
using WalkProject.API.GraphQL.DTOs.Users;
using WalkProject.API.GraphQL.Resolvers;
using WalkProject.DataModels.Entities;
using WalkProject.Middlewares.Users;

namespace WalkProject.API.GraphQL.Schemas.Mutations
{
    [ExtendObjectType("Mutation")]
    public class UserMutation
    {
        private readonly UsersResolver _resolver;
        private readonly IMapper mapper;

        public UserMutation(UsersResolver resolver, IMapper mapper)
        {
            _resolver = resolver;
            this.mapper = mapper;
        }

        // Update Profile
        [Authorize]
        [UseUser]
        public async Task<User> UpdateProfile([User] User user, UserProfileInput userProfileInput)
        {
            var userDomain = mapper.Map<User>(userProfileInput);

            // check if the user exists
            userDomain = await _resolver.UpdateAsync(user.IdentityId, userDomain);

            if (userDomain == null)
            {
                throw new GraphQLException(new Error("User not found.", "USER_NOT_FOUND"));
            }

            return userDomain;
        }

        // Update Role
        [Authorize(Policy = "IsAdmin")]
        public async Task<User> UpdateRole(Guid userId, Guid roleId)
        {
            var userRole = await _resolver.GetRoleNameByIdAsync(userId);

            if (userRole == "admin")
            {
                throw new GraphQLException(new Error("Can not change role Admin.", "ROLE_CANNOT_CHANGE"));
            }

            var role = await _resolver.GetRoleByIdAsync(roleId);

            if (role == null || role.Name == "admin")
            {
                throw new GraphQLException(new Error("Role not valid.", "ROLE_NOT_VALID"));
            }

            var userDomainModel = await _resolver.UpdateRoleAsync(userId, roleId);

            if (userDomainModel == null)
            {
                throw new GraphQLException(new Error("User not found.", "USER_NOT_FOUND"));
            }

            return userDomainModel;
        }
    }
}
