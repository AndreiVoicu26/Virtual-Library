using PersonBook.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PersonBook.Web.Areas.Person.Pages
{
    public class EditModel : PageModel
    {
        [BindProperty] public string FirstName { get; set; }
        [BindProperty] public string LastName { get; set; }
        [BindProperty] public DateOnly DateOfBirth { get; set; }        
        [BindProperty] public IList<string> ErrorMessages { get; set; }

        private readonly IPersonRepository personRepository;
        
        public EditModel(IPersonRepository personRepository)
        {
            this.personRepository = personRepository;            
            ErrorMessages = new List<string>();
        }

        public async Task<IActionResult> OnGetAsync(Guid Id)
        {            
            var person = await personRepository.GetPersonById(Id);
            FirstName = person.FirstName;
            LastName = person.LastName;
            DateOfBirth = person.DateOfBirth;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid Id)
        {
            var res1 = await personRepository.SetPersonFirstNameAsync(Id, FirstName);
            if (!res1.Success)
            {
                ErrorMessages.Add($"Error setting name: {res1.Reason}");
            }

            var res2 = await personRepository.SetPersonLastNameAsync(Id, LastName);
            if (!res2.Success)
            {
                ErrorMessages.Add($"Error setting name: {res2.Reason}");
            }

            var res3= await personRepository.SetPersonDateOfBirthAsync(Id, DateOfBirth);
            if (!res3.Success)
            {
                ErrorMessages.Add($"Error setting age: {res3.Reason}");
            }

            if (ErrorMessages.Any()) return Page();
            else return RedirectToPage("Index");
        }
    }
}
