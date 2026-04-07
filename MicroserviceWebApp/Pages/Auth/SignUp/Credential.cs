using System.Text.Json.Serialization;

namespace MicroserviceWebApp.Pages.Auth.SignUp
{
    public record Credential(
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("value")] string Value,
        [property: JsonPropertyName("temporary")] bool Temporary
    );
}
