using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonBook.Core.Info;
using PersonBook.Core.Repositories;

namespace PersonBook.Web.Areas.Book.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty] public IEnumerable<BookInfo> Books { get; set; }
        [BindProperty] public IEnumerable<string> Errors { get; set; }

        private readonly IBookRepository bookRepository;

        public IndexModel(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var DbBooks = await bookRepository.GetBooks();
            Books = DbBooks.OrderByDescending(o => o.LastUpdated);
            return Page();
        }
    }
}
