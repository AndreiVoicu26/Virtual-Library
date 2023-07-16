using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PersonBook.Core.Repositories;
using PersonBook.Core.Info;

namespace PersonBook.Web.Areas.Person.Pages
{
    public class EditModel : PageModel
    {
        [BindProperty] public string FirstName { get; set; }
        [BindProperty] public string LastName { get; set; }
        [BindProperty] public DateOnly DateOfBirth { get; set; }
        public IEnumerable<BookInfo> AvailableBooks { get; set; }
        public IEnumerable<BookInfo> BorrowedBooks { get; set; }
        public IList<BookInfo> SelectedBooksToAdd { get; set; } = new List<BookInfo>();
        public IList<BookInfo> SelectedBooksToRemove { get; set; } = new List<BookInfo>();
        [BindProperty] public IList<string> ErrorMessages { get; set; }

        private readonly IPersonRepository personRepository;

        private readonly IBookRepository bookRepository;
        
        public EditModel(IPersonRepository personRepository, IBookRepository bookRepository)
        {
            this.personRepository = personRepository;      
            this.bookRepository = bookRepository;
            ErrorMessages = new List<string>();
        }

        public async Task<IActionResult> OnGetAsync(Guid Id)
        {            
            var person = await personRepository.GetPersonById(Id);
            var availableBooks = await bookRepository.GetAvailableBooks();
            var borrowedBooks = await personRepository.GetBorrowedBooks(Id);
            FirstName = person.FirstName;
            LastName = person.LastName;
            DateOfBirth = person.DateOfBirth;
            AvailableBooks = availableBooks;
            BorrowedBooks = borrowedBooks;
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

            var res3 = await personRepository.SetPersonDateOfBirthAsync(Id, DateOfBirth);
            if (!res3.Success)
            {
                ErrorMessages.Add($"Error setting age: {res3.Reason}");
            }

            var AvailableBooks = await bookRepository.GetAvailableBooks();

            var BorrowedBooks = await personRepository.GetBorrowedBooks(Id);

            foreach (var b in AvailableBooks)
            {
                if (Request.Form[b.Id.ToString() + "-borrow"] == "on")
                {
                    SelectedBooksToAdd.Add(b);
                }
            }

            var res4 = await personRepository.BorrowBooksAsync(Id, SelectedBooksToAdd);
            if (!res4.Success)
            {
                ErrorMessages.Add($"Error borrowing the book: {res4.Reason}");
            }

            foreach (var b in BorrowedBooks)
            {
                if (Request.Form[b.Id.ToString() + "-borrowed"] != "on")
                {
                    SelectedBooksToRemove.Add(b);
                }
            }

            var res5 = await personRepository.ReturnBooksAsync(Id, SelectedBooksToRemove);
            if (!res5.Success)
            {
                ErrorMessages.Add($"Error returning the book: {res5.Reason}");
            }

            if (ErrorMessages.Any()) return Page();
            else return RedirectToPage("Index");
        }
    }
}
