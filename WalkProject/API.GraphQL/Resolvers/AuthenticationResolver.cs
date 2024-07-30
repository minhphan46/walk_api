using FirebaseAdmin.Auth;
using WalkProject.API.GraphQL.DTOs.Authentication;
using WalkProject.Utils;

namespace WalkProject.API.GraphQL.Resolvers
{
    public class AuthenticationResolver
    {
        private readonly JwtProvider _provider;

        public AuthenticationResolver(JwtProvider provider)
        {
            _provider = provider;
        }

        public async Task<UserRecord> RegisterAsyc(string email, string password)
        {
            var userArgs = new UserRecordArgs
            {
                Email = email,
                Password = password
            };

            var userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(userArgs);

            return userRecord;
        }

        public async Task<LoginResponse> LoginAsyc(string email, string password)
        {
            var userArgs = new UserRecordArgs
            {
                Email = email,
                Password = password
            };

            var respone = await _provider.GetForCredentialsAsync(email, password);

            return respone;
        }
    }
}
