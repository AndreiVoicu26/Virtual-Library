using PersonBook.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;

namespace PersonBook.Web.Areas.Person.Pages
{
    public class CreateModel : PageModel
    {        
        [BindProperty] public string FirstName { get; set; }
        [BindProperty] public string LastName { get; set; }
        [BindProperty] public DateOnly DateOfBirth { get; set; }                
        [BindProperty] public IList<string> ErrorMessages { get; set; }

        private readonly IPersonRepository personRepository;   

        public CreateModel(IPersonRepository personRepository)
        {
            this.personRepository = personRepository;            
            ErrorMessages = new List<string>();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var res1 = await personRepository.AddPersonAsync(FirstName, LastName);
            if (!res1.Success)
            {
                ErrorMessages.Add($"Error creating person: {res1.Reason}");
            }

            var res2 = await personRepository.SetPersonDateOfBirthAsync(res1.Id, DateOfBirth);
            if (!res2.Success)
            {
                ErrorMessages.Add($"Error setting age: {res2.Reason}");
            }

            if (ErrorMessages.Any()) return Page();
            else return RedirectToPage("Index");
        }
    }

    public record PersonViewModel(Guid Id, string FirstName, string LastName, DateOnly DateOfBirth, DateTime LastUpdatedOn);
}
