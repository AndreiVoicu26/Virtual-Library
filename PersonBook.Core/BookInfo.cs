namespace PersonBook.Core
{
    public record BookInfo(Guid Id, string Title, string Author, string Isbn, short Year, DateTime LastUpdated);
}
