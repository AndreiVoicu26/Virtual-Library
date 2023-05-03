using PersonBook.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PersonBook.Web.Areas.Person.Pages
{
    public class EditModel : PageModel
    {
        [BindProperty] public string Name { get; set; }
        [BindProperty] public int Age { get; set; }        
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
            Name = person.Name;
            Age = person.Age;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid Id)
        {
            var res1 = await personRepository.SetPersonNameAsync(Id, Name);
            if (!res1.Success)
            {
                ErrorMessages.Add($"Error setting name: {res1.Reason}");
            }

            var res2= await personRepository.SetPersonAgeAsync(Id, Age);
            if (!res2.Success)
            {
                ErrorMessages.Add($"Error setting age: {res2.Reason}");
            }

            if (ErrorMessages.Any()) return Page();
            else return RedirectToPage("Index");
        }
    }
}
