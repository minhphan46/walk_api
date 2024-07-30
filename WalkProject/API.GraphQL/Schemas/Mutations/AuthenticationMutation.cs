using WalkProject.API.GraphQL.DTOs.Authentication;
using WalkProject.API.GraphQL.Resolvers;
using WalkProject.DataModels.Entities;

namespace WalkProject.API.GraphQL.Schemas.Mutations
{
    [ExtendObjectType("Mutation")]
    public class AuthenticationMutation
    {
        private readonly AuthenticationResolver authResolver;
        private readonly UsersResolver usersResolver;

        public AuthenticationMutation(AuthenticationResolver authResolver, UsersResolver usersResolver)
        {
            this.authResolver = authResolver;
            this.usersResolver = usersResolver;
        }

        public async Task<LoginResponse> Login(LoginInput loginInput)
        {
            var respone = await authResolver.LoginAsyc(loginInput.Email, loginInput.Password);

            if (respone.AccessToken == null || respone.RefreshToken == null)
            {
                throw new GraphQLException(new Error("User not found.", "USER_NOT_FOUND"));
            }
            return respone;
        }

        public async Task<string> SignUp(RegisterInput registerInput)
        {
            var userRecord = await authResolver.RegisterAsyc(registerInput.Email, registerInput.Password);

            var newUser = new User()
            {
                IdentityId = userRecord.Uid,
                Email = userRecord.Email,
                EmailVerified = userRecord.EmailVerified,
                Username = userRecord.DisplayName,
                RoleId = new Guid("bd8e2129-6568-4c38-b15a-b9fbdb64dc31")
            };

            await usersResolver.CreateAsync(newUser);

            return "User was registered! Please login.";
        }
    }
}
