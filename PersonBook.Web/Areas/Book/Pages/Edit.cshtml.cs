using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonBook.Core.Repositories;

namespace PersonBook.Web.Areas.Book.Pages
{
    public class EditModel : PageModel
    {
        [BindProperty] public string Title { get; set; }
        [BindProperty] public string Author { get; set; }
        [BindProperty] public string Isbn { get; set; }
        [BindProperty] public short Year { get; set; }
        [BindProperty] public IList<string> ErrorMessages { get; set; }

        private readonly IBookRepository bookRepository;

        public EditModel(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
            ErrorMessages = new List<string>();
        }

        public async Task<IActionResult> OnGetAsync(Guid Id)
        {
            var book = await bookRepository.GetBookById(Id);
            Title = book.Title;
            Author = book.Author;
            Isbn = book.Isbn;
            Year = book.Year;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid Id)
        {
            var res1 = await bookRepository.SetBookTitleAsync(Id, Title);
            if (!res1.Success)
            {
                ErrorMessages.Add($"Error setting name: {res1.Reason}");
            }

            var res2 = await bookRepository.SetBookAuthorAsync(Id, Author);
            if (!res2.Success)
            {
                ErrorMessages.Add($"Error setting name: {res2.Reason}");
            }

            var res3 = await bookRepository.SetBookIsbnAsync(Id, Isbn);
            if (!res3.Success)
            {
                ErrorMessages.Add($"Error setting name: {res3.Reason}");
            }

            var res4 = await bookRepository.SetBookYearAsync(Id, Year);
            if (!res4.Success)
            {
                ErrorMessages.Add($"Error setting name: {res4.Reason}");
            }

            if (ErrorMessages.Any()) return Page();
            else return RedirectToPage("Index");
        }

    }
}
