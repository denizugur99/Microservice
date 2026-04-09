using MicroserviceWebApp.Pages.Auth.SignUp;
using System.ComponentModel.DataAnnotations;

namespace MicroserviceWebApp.Pages.Auth.SignIn
{
    public class SignInViewModel
    {
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; init; } = string.Empty;

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; init; } = string.Empty;
        public static SignInViewModel Empty => new();
        public static SignInViewModel GetExampleModel => new()
        {
          
            Email = "buzzer@gmail.com",
            Password = "Password123",
          
        };
    }
}
