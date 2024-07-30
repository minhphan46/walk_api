using WalkProject.API.GraphQL.DTOs.Authentication;
using System.Text.Json.Serialization;

namespace WalkProject.Utils
{
    public class JwtProvider
    {
        private readonly HttpClient _httpClient;

        public JwtProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResponse> GetForCredentialsAsync(string email, string password)
        {
            var request = new
            {
                email,
                password,
                returnSecureToken = true
            };

            var respone = await _httpClient.PostAsJsonAsync("", request);

            var authToken = await respone.Content.ReadFromJsonAsync<AuthToken>();

            return new LoginResponse
            {
                AccessToken = authToken.IdToken,
                RefreshToken = authToken.RefreshToken
            };
        }

        public class AuthToken
        {
            [JsonPropertyName("kind")]
            public string Kind { get; set; }

            [JsonPropertyName("localId")]
            public string LocalId { get; set; }

            [JsonPropertyName("email")]
            public string Email { get; set; }

            [JsonPropertyName("displayName")]
            public string DisplayName { get; set; }

            [JsonPropertyName("idToken")]
            public string IdToken { get; set; }

            [JsonPropertyName("registered")]
            public bool Registered { get; set; }

            [JsonPropertyName("refreshToken")]
            public string RefreshToken { get; set; }

            [JsonPropertyName("expiresIn")]
            public string ExpiresIn { get; set; }
        }
    }
}
