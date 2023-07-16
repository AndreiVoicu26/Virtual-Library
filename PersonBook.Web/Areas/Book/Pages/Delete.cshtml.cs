using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonBook.Core.Repositories;

namespace PersonBook.Web.Areas.Book.Pages
{
    public class DeleteModel : PageModel
    {
        [BindProperty] public string Title { get; set; }
        [BindProperty] public string Author { get; set; }
        [BindProperty] public IList<string> ErrorMessages { get; set; }

        private readonly IBookRepository bookRepository;

        public DeleteModel(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
            ErrorMessages = new List<string>();
        }

        public async Task<IActionResult> OnGetAsync(Guid Id)
        {
            var book = await bookRepository.GetBookById(Id);
            Title = book.Title;
            Author = book.Author;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid Id)
        {
            var res = await bookRepository.RemoveBookAsync(Id);
            if (!res.Success)
            {
                ErrorMessages.Add($"Error deleting book: {res.Reason}");
            }

            if (ErrorMessages.Any()) return Page();
            else return RedirectToPage("Index");
        }
    }
}
