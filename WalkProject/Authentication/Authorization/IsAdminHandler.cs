using FirebaseAdminAuthentication.DependencyInjection.Models;
using Microsoft.AspNetCore.Authorization;
using WalkProject.API.GraphQL.Resolvers;
using WalkProject.Authentication.Authorization.Rules;

namespace WalkProject.Authentication.Authorization
{
    public class IsAdminHandler : AuthorizationHandler<IsAllowDeleted>
    {
        private readonly UsersResolver resolver;

        public IsAdminHandler(UsersResolver resolver)
        {
            this.resolver = resolver;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAllowDeleted requirement)
        {
            // Kiểm tra nếu người dùng có claim với loại FirebaseUserClaimType.ID
            var userIdClaim = context.User.FindFirst(c => c.Type == FirebaseUserClaimType.ID);

            if (userIdClaim != null)
            {
                var roleName = await resolver.GetRoleNameByIdentityIdAsync(userIdClaim.Value);

                if (roleName == "admin")
                {
                    // Claim tồn tại và vai trò là admin, đáp ứng yêu cầu
                    context.Succeed(requirement);
                }
            }
        }
    }
}
