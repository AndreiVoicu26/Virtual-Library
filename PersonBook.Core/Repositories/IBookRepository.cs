using PersonBook.Core.Data;
using PersonBook.Core.Info;

namespace PersonBook.Core.Repositories
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
        Task<IEnumerable<BookInfo>> GetAvailableBooks();
        Task<IEnumerable<PersonInfo>> GetOwnersOfBook(Guid Id);
    }
}
