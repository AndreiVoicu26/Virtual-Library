using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using PersonBook.Core.Repositories;
using PersonBook.Core.Info;

namespace PersonBook.Web.Areas.Person.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty] public IEnumerable<PersonInfo> Persons { get; set; }
        [BindProperty] public IEnumerable<string> Errors { get; set; }

        private readonly IPersonRepository personRepository;

        public IndexModel(IPersonRepository personRepository)
        {
            this.personRepository = personRepository;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var DbPers = await personRepository.GetPersons();
            Persons = DbPers.OrderByDescending(o => o.LastUpdatedOn);
            return Page();
        }
    }
}
