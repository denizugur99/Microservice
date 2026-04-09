using MicroserviceWebApp.Pages.Auth.SignIn;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;

namespace MicroserviceWebApp.Pages.Auth
{
    public class SignInModel(SignInService signInService) : PageModel
    {
        [BindProperty]
        public required SignInViewModel Input { get; set; } = SignInViewModel.GetExampleModel;
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var result = await signInService.SignInAsync(Input);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.ProblemDetails.Title);
                ModelState.AddModelError(string.Empty, result.ProblemDetails.Detail);

                return Page();
            }
            return RedirectToPage("/Index");
        }
    }
}