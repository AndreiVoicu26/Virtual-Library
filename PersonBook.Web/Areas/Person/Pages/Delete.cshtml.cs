using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonBook.Core.Repositories;

namespace PersonBook.Web.Areas.Person.Pages
{
    public class DeleteModel : PageModel
    {
        [BindProperty] public string FirstName { get; set; }
        [BindProperty] public string LastName { get; set; }
        [BindProperty] public IList<string> ErrorMessages { get; set; }

        private readonly IPersonRepository personRepository;

        public DeleteModel(IPersonRepository personRepository)
        {
            this.personRepository = personRepository;
            ErrorMessages = new List<string>();
        }

        public async Task<IActionResult> OnGetAsync(Guid Id)
        {
            var info = await personRepository.GetPersonById(Id);
            FirstName = info.FirstName;
            LastName = info.LastName;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid Id)
        {
            var res = await personRepository.RemovePersonAsync(Id);
            if (!res.Success)
            {
                ErrorMessages.Add($"Error deleting person: {res.Reason}");
            }
            
            if (ErrorMessages.Any()) return Page();
            else return RedirectToPage("Index");
        }
    }
}
