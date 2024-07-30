using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WalkProject.API.RestFul.DTOs.ApiResponse;
using WalkProject.API.RestFul.DTOs.AuthenticationModel;
using WalkProject.API.RestFul.Repositories.Interfaces;

namespace NZWalks.RESTful.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username,
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (!identityResult.Succeeded)
            {
                return BadRequest("Identity result failure");
            }

            // Add roles to this User
            if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
            {
                identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                if (identityResult.Succeeded)
                {
                    var apiRespone = new APISucessResponse(
                            statusCode: System.Net.HttpStatusCode.Created,
                            message: "User was registered! Please login.",
                            data: null
                        );
                    return Ok(apiRespone);
                }
            }

            return BadRequest("Something went wrong");
        }

        // POST: /api/Auth/Login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);

            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (!checkPasswordResult)
                {
                    return BadRequest("Password incorrect");
                }

                // Get Roles for this user
                var roles = await userManager.GetRolesAsync(user);

                if (roles != null)
                {
                    // Create Token

                    var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());

                    var response = new LoginResponseDto
                    {
                        AccessToken = jwtToken
                    };

                    var apiRespone = new APISucessResponse(
                            statusCode: System.Net.HttpStatusCode.OK,
                            message: "Successfully login",
                            data: response
                        );

                    return Ok(apiRespone);
                }

            }

            return BadRequest("Username or password incorrect");
        }
    }
}
