using Microsoft.AspNetCore.Identity;

namespace WalkProject.API.RestFul.Repositories.Interfaces
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
