using PersonBook.Core.Models;

namespace PersonBook.Core.Info
{
    public record PersonInfo(Guid Id, string FirstName, string LastName, DateOnly DateOfBirth, IList<BookInfo> Books, DateTime LastUpdatedOn);
}