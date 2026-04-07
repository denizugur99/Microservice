using System.ComponentModel.DataAnnotations;

namespace MicroserviceWebApp.Pages.Auth.SignUp
{
    public record SignUpViewModel(
        [Display(Name = "Username")] string User,
        [Display(Name = "First Name")] string FirstName,
        [Display(Name = "Last Name")] string LastName,
        [Display(Name = "Email")] string Email,
        [Display(Name = "Password")] string Password,
        [Display(Name = "Password Confirm")] string ConfirmPassword
    )
    {
        public static SignUpViewModel Empty => new(string.Empty,string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        public static SignUpViewModel GetExampleModel=> new("jhonny","John", "Doe", "denougur0@gmail.com", "Password123", "Password123");
    }

   
}
   
