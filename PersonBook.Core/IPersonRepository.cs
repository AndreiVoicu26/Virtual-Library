using PersonBook.Core.Data;

namespace PersonBook.Core
{
    public interface IPersonRepository
    {
        Task<DbResult> AddPersonAsync(string Name);
        Task<DbResult> SetPersonNameAsync(Guid Id, string Name);
        Task<DbResult> SetPersonAgeAsync(Guid Id, int Age);
        Task<DbResult> RemovePersonAsync(Guid Id);
        Task<IEnumerable<PersonInfo>> GetPersons();
        Task<PersonInfo> GetPersonById(Guid Id);        
    }
}