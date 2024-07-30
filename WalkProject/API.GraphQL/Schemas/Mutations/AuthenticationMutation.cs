using WalkProject.API.GraphQL.DTOs.Authentication;
using WalkProject.API.GraphQL.Resolvers;

namespace WalkProject.API.GraphQL.Schemas.Mutations
{
    [ExtendObjectType("Mutation")]
    public class AuthenticationMutation
    {
        private readonly AuthenticationResolver resolver;

        public AuthenticationMutation(AuthenticationResolver resolver)
        {
            this.resolver = resolver;
        }

        public async Task<LoginResponse> Login(LoginInput loginInput)
        {
            var respone = await resolver.LoginAsyc(loginInput.Email, loginInput.Password);

            if (respone.AccessToken == null || respone.RefreshToken == null)
            {
                throw new GraphQLException(new Error("User not found.", "USER_NOT_FOUND"));
            }
            return respone;
        }

        public async Task<string> SignUp(RegisterInput registerInput)
        {
            await resolver.RegisterAsyc(registerInput.Email, registerInput.Password);

            return "User was registered! Please login.";
        }
    }
}
