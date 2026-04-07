using System.Text.Json.Serialization;

namespace MicroserviceWebApp.Pages.Auth.SignUp
{
    public record UserCreateRequest(
        [property: JsonPropertyName("username")] string UserName,
        [property: JsonPropertyName("enabled")] bool Enabled,
        [property: JsonPropertyName("firstName")] string FirstName,
        [property: JsonPropertyName("lastName")] string LastName,
        [property: JsonPropertyName("email")] string Email,
        [property: JsonPropertyName("credentials")] List<Credential> Credentials
    );
}
