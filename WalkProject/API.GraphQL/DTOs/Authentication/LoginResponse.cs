namespace WalkProject.API.GraphQL.DTOs.Authentication
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
