using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonBook.Core.Info;
using PersonBook.Core.Models;
using PersonBook.Core.Repositories;

namespace PersonBook.Web.Areas.Person.Pages
{
    public class DetailsModel : PageModel
    {
        [BindProperty] public string FirstName { get; set; }
        [BindProperty] public string LastName { get; set; }
        [BindProperty] public IEnumerable<BookInfo> Books { get; set; }
        [BindProperty] public IEnumerable<string> Errors { get; set; }

        private readonly IPersonRepository personRepository;

        public DetailsModel(IPersonRepository personRepository)
        {
            this.personRepository = personRepository;
        }

        public async Task<IActionResult> OnGetAsync(Guid Id)
        {
            var person = await personRepository.GetPersonById(Id);
            var books = await personRepository.GetBorrowedBooks(Id);
            FirstName = person.FirstName;
            LastName = person.LastName;
            Books = books.OrderByDescending(o => o.LastUpdated);
            return Page();
        }
    }
}
