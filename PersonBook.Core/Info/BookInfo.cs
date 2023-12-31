﻿namespace PersonBook.Core.Info
{
    public record BookInfo(Guid Id, string Title, string Author, string Isbn, short Year, bool IsAvailable, IList<PersonInfo> Owners, DateTime LastUpdated);
}
