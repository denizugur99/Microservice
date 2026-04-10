using Duende.IdentityModel.Client;
using MicroserviceWebApp.Options.IdentityOptions;
using MicroserviceWebApp.Services;
using Microsoft.AspNetCore.Identity;

namespace MicroserviceWebApp.Pages.Auth.SignUp
{
    public record KeyCloakError(string ErrorMessage);

    public class SignUpService(IdentityOption identityOption, HttpClient httpClient,ILogger<SignUpService>logger)
    {
        public async Task<ServiceResult> CreateAccount(SignUpViewModel model)
        {
            var token = await GetClientCredentialTokenAsAdmin();
            var address = $"{identityOption.BaseAddress}/admin/realms/microserviceTenant/users";
            httpClient.SetBearerToken(token);
            var userCreateRequest = CreateUserRequest(model);
            var response = await httpClient.PostAsJsonAsync(address, userCreateRequest);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode != System.Net.HttpStatusCode.InternalServerError)
                {
                    var errorContent = await response.Content.ReadFromJsonAsync<KeyCloakError>();

                    return ServiceResult.Error("Account Creation Failed", errorContent!.ErrorMessage);
                }
                var errorMessage = await response.Content.ReadAsStringAsync();
                logger.LogError(errorMessage);
                return ServiceResult.Error("Account Creation Failed", "An error occurred while creating the account. Please try again later.");

            }
            return ServiceResult.Success();

        }
        private static UserCreateRequest CreateUserRequest(SignUpViewModel model)
        {
            return new UserCreateRequest(model.User, true, model.FirstName, model.LastName, model.Email, new List<Credential>()
            {
                new Credential("password",model.Password,false)
            });
        }
       private async Task<string> GetClientCredentialTokenAsAdmin()
        {
            var discoveryRequest = new DiscoveryDocumentRequest()
            {
                Address = identityOption.Address,
                Policy =
                    {
                        RequireHttps=false
                    }
            };
            var client = httpClient;
            client.BaseAddress = new Uri(identityOption.Address);
            var discoveryResponse = await client.GetDiscoveryDocumentAsync();
            if (discoveryResponse.IsError)
            {
                throw new Exception($"Identity Server Discovery Failed:{discoveryResponse.Error}");
            }
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryResponse.TokenEndpoint,
                ClientId = identityOption.Admin.ClientId,
                ClientSecret = identityOption.Admin.ClientSecret,
            });
            if (tokenResponse.IsError)
            {
                throw new Exception($"Token Request Failed:{tokenResponse.Error}");
            }
         return tokenResponse.AccessToken!;
        }
    }
}
