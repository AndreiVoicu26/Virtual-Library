namespace PersonBook.Core
{
    public record PersonInfo(Guid Id, string FirstName, string LastName, DateOnly DateOfBirth, DateTime LastUpdatedOn);
}