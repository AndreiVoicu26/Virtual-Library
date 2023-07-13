using PersonBook.Core.Data;

namespace PersonBook.Core
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
    }
}