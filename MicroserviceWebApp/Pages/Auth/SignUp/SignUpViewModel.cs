using System.ComponentModel.DataAnnotations;

namespace MicroserviceWebApp.Pages.Auth.SignUp
{
    public record SignUpViewModel
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is required")]
        public string User { get; init; } = string.Empty;

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; init; } = string.Empty;

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; init; } = string.Empty;

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; init; } = string.Empty;

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; init; } = string.Empty;

        [Display(Name = "Password Confirm")]
        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; init; } = string.Empty;

        public static SignUpViewModel Empty => new();
        public static SignUpViewModel GetExampleModel => new()
        {
            User = "jhonny",
            FirstName = "John",
            LastName = "Doe",
            Email = "denougur0@gmail.com",
            Password = "Password123",
            ConfirmPassword = "Password123"
        };
    }

   
}
   
