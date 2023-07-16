using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonBook.Core.Repositories;

namespace PersonBook.Web.Areas.Book.Pages
{
    public class CreateModel : PageModel
    {
        [BindProperty] public string Title { get; set; }
        [BindProperty] public string Author { get; set; }
        [BindProperty] public string Isbn { get; set; }
        [BindProperty] public short Year { get; set; }
        [BindProperty] public IList<string> ErrorMessages { get; set; }

        private readonly IBookRepository bookRepository;

        public CreateModel(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
            ErrorMessages = new List<string>();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var res = await bookRepository.AddBookAsync(Title, Author, Isbn, Year);
            if (!res.Success)
            {
                ErrorMessages.Add($"Error creating person: {res.Reason}");
            }

            if (ErrorMessages.Any()) return Page();
            else return RedirectToPage("Index");
        }
    }

    public record BookViewModel(Guid Id, string Title, string Author, string Isbn, short Year, bool IsAvailable, DateTime LastUpdated);
}
