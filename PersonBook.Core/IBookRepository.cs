using PersonBook.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonBook.Core
{
    public interface IBookRepository
    {
        Task<DbResult> AddBookAsync(string Title, string Author, string Isbn, short Year);
        Task<DbResult> SetBookTitleAsync(Guid Id, string Title);
        Task<DbResult> SetBookAuthorAsync(Guid Id, string Author);
        Task<DbResult> SetBookIsbnAsync(Guid Id, string Isbn);
        Task<DbResult> SetBookYearAsync(Guid Id, short Year);
        Task<DbResult> RemoveBookAsync(Guid Id);
        Task<IEnumerable<BookInfo>> GetBooks();
        Task<BookInfo> GetBookById(Guid Id);
    }
}
