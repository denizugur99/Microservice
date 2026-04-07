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
            var result =await signUpService.CreateAccount(Input);
            if (result.IsFail)
            {
                return Page();

            }
            else
            {
                return RedirectToPage("/Index");
            }
        }

      
    }

    
}
