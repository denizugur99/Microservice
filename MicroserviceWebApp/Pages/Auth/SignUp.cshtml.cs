using MicroserviceWebApp.Pages.Auth.SignUp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MicroserviceWebApp.Pages.Auth
{
    public class SignUpModel(SignUpService signUpService) : PageModel
    {
        [BindProperty]
        public SignUpViewModel Input { get; set; }= SignUpViewModel.GetExampleModel;

  

        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var result =await signUpService.CreateAccount(Input);
            if (result.IsFail)
            {
                if (!string.IsNullOrEmpty(result.ProblemDetails.Title))
                {
                    ModelState.AddModelError(string.Empty, result.ProblemDetails.Title);
                }
                if(!string.IsNullOrEmpty(result.ProblemDetails.Detail))
                {
                    ModelState.AddModelError(string.Empty, result.ProblemDetails.Detail);
                }
                return Page();

            }

                return RedirectToPage("/Index");

        }

      
    }

    
}
