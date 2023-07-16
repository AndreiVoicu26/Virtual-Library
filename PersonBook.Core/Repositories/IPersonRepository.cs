using PersonBook.Core.Data;
using PersonBook.Core.Info;

namespace PersonBook.Core.Repositories
{
    public interface IPersonRepository
    {
        Task<DbResult> AddPersonAsync(string FirstName, string LastName);
        Task<DbResult> SetPersonFirstNameAsync(Guid Id, string FirstName);
        Task<DbResult> SetPersonLastNameAsync(Guid Id, string LastName);
        Task<DbResult> SetPersonDateOfBirthAsync(Guid Id, DateOnly DateOfBirth);
        Task<DbResult> RemovePersonAsync(Guid Id);
        Task<IEnumerable<PersonInfo>> GetPersons();
        Task<PersonInfo> GetPersonById(Guid Id);
        Task<IEnumerable<BookInfo>> GetBorrowedBooks(Guid Id);
        Task<DbResult> BorrowBooksAsync(Guid Id, IEnumerable<BookInfo> books);
        Task<DbResult> ReturnBooksAsync(Guid Id, IEnumerable<BookInfo> books);
    }
}