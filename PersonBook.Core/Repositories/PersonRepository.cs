using PersonBook.Core.Data;
using PersonBook.Core.Models;
using Mapster;
using MongoDB.Bson;
using MongoDB.Driver;
using PersonBook.Core.Info;

namespace PersonBook.Core.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly DataContext context;

        public PersonRepository(DataContext context)
        {
            this.context = context;
        }
        /// <summary>
        /// Create a new person
        /// </summary>
        /// <param name="FirstName">Person's first name</param> 
        /// <param name="LastName">Person's last name</param>
        /// <returns>A DbResult structure containing the result of the database operation</returns>        
        public async Task<DbResult> AddPersonAsync(string FirstName, string LastName)
        {
            var Persons = await GetPersons();
            if (Persons.Any(a => a.FirstName.Equals(FirstName) && a.LastName.Equals(LastName)))
            {
                return DbResult.Fail($"A person named '{FirstName} {LastName}' already exists.");
            }
            var newDoc = new PersonDoc
            {
                FirstName = FirstName,
                LastName = LastName,
                LastUpdatedOn = DateTime.UtcNow.ToLocalTime()
            };
            try
            {
                await context.PersonCollection.InsertOneAsync(newDoc);
                return DbResult.Succeed(newDoc.Id);
            }
            catch (Exception ex)
            {
                return DbResult.Fail(ex);
            }
        }

        /// <summary>
        /// Set the person name
        /// </summary>
        /// <param name="Id">Person id</param>
        /// <param name="FirstName">Person first name</param>
        /// <returns>A DbResult structure containing the result of the database operation</returns>
        public async Task<DbResult> SetPersonFirstNameAsync(Guid Id, string FirstName)
        {
            var res = await context.PersonCollection.UpdateOneAsync(r => r.Id.Equals(Id),
                Builders<PersonDoc>
                .Update
                .Set(r => r.LastUpdatedOn, DateTime.UtcNow.ToLocalTime())
                .Set(r => r.FirstName, FirstName));
            return res.IsAcknowledged ? DbResult.Succeed() : DbResult.Fail();
        }

        /// <summary>
        /// Set the person name
        /// </summary>
        /// <param name="Id">Person id</param>
        /// <param name="LastName">Person last name</param>
        /// <returns>A DbResult structure containing the result of the database operation</returns>
        public async Task<DbResult> SetPersonLastNameAsync(Guid Id, string LastName)
        {
            var res = await context.PersonCollection.UpdateOneAsync(r => r.Id.Equals(Id),
                Builders<PersonDoc>
                .Update
                .Set(r => r.LastUpdatedOn, DateTime.UtcNow.ToLocalTime())
                .Set(r => r.LastName, LastName));
            return res.IsAcknowledged ? DbResult.Succeed() : DbResult.Fail();
        }

        /// <summary>
        /// Set the person age
        /// </summary>
        /// <param name="Id">Person id</param>
        /// <param name="DateOfBirth">Person date of birth</param>        
        /// <returns>A DbResult structure containing the result of the database operation</returns>
        public async Task<DbResult> SetPersonDateOfBirthAsync(Guid Id, DateOnly DateOfBirth)
        {
            var res = await context.PersonCollection.UpdateOneAsync(r => r.Id.Equals(Id),
                Builders<PersonDoc>
                .Update
                .Set(r => r.LastUpdatedOn, DateTime.UtcNow.ToLocalTime())
                .Set(r => r.DateOfBirth, DateOfBirth));
            return res.IsAcknowledged ? DbResult.Succeed() : DbResult.Fail();
        }

        /// <summary>
        /// Get all registered persons.
        /// </summary>
        /// <returns>An enumeration of PersonInfo objects</returns>
        public async Task<IEnumerable<PersonInfo>> GetPersons()
        {
            var dbObjs = await context.PersonCollection.FindAsync(new BsonDocument());
            return dbObjs.ToList().Adapt<IEnumerable<PersonInfo>>();
        }

        /// <summary>
        /// Get person by id
        /// </summary>
        /// <param name="Id">Person id</param>
        /// <returns>A PersonInfo object</returns>
        public async Task<PersonInfo> GetPersonById(Guid Id)
        {
            var filter = Builders<PersonDoc>.Filter.Eq(m => m.Id, Id);
            var dbObjs = (await context.PersonCollection.FindAsync(filter)).FirstOrDefaultAsync().Result;
            return dbObjs.Adapt<PersonInfo>();
        }

        /// <summary>
        /// Remove a specified person
        /// </summary>
        /// <param name="Id">Person id</param>
        /// <returns>A DbResult structure containing the result of the database operation</returns>
        public async Task<DbResult> RemovePersonAsync(Guid Id)
        {
            var filter = Builders<PersonDoc>.Filter.Eq(m => m.Id, Id);
            var person = (await context.PersonCollection.FindAsync(filter)).FirstOrDefaultAsync().Result;
            if (person.Books != null && person.Books.Any())
            {
                return DbResult.Fail($"'{person.FirstName} {person.LastName}' has unreturned books.");
            }
            try
            {
                await context.PersonCollection.DeleteOneAsync(s => s.Id.Equals(Id));
                return DbResult.Succeed();
            }
            catch (Exception ex)
            {
                return DbResult.Fail(ex);
            }
        }

        /// <summary>
        /// Get all books borrowed by a specified person
        /// </summary>
        /// <param name="Id">Persoon id</param>
        /// <returns>An enumeration of BookInfo objects</returns>
        public async Task<IEnumerable<BookInfo>> GetBorrowedBooks(Guid Id)
        {
            var filter_person = Builders<PersonDoc>.Filter.Eq(m => m.Id, Id);
            var person = (await context.PersonCollection.FindAsync(filter_person)).FirstOrDefaultAsync().Result;
            if (person.Books != null)
            {
                return person.Books.ToList().Adapt<IEnumerable<BookInfo>>();
            }
            else
            {
                return Enumerable.Empty<BookInfo>();
            }
        }

        /// <summary>
        /// Borrow a list of books
        /// </summary>
        /// <param name="Id">Person id</param>
        /// <param name="SelectedBooks">Selected books to borrow</param>
        /// <returns>A DbResult structure containing the result of the database operation</returns>
        public async Task<DbResult> BorrowBooksAsync(Guid Id, IEnumerable<BookInfo> SelectedBooks)
        {
            var filter_person = Builders<PersonDoc>.Filter.Eq(m => m.Id, Id);
            var person = (await context.PersonCollection.FindAsync(filter_person)).FirstOrDefaultAsync().Result;
            var books = person.Books;
            books ??= new List<BookInfo>();
            foreach (var book in SelectedBooks)
            {
                var owners = book.Owners;
                owners ??= new List<PersonInfo>();
                owners.Add(person.Adapt<PersonInfo>());
                var res1 = await context.BookCollection.UpdateOneAsync(b => b.Id.Equals(book.Id),
                    Builders<BookDoc>
                    .Update
                    .Set(r => r.LastUpdated, DateTime.UtcNow.ToLocalTime())
                    .Set(r => r.IsAvailable, false)
                    .Set(r => r.Owners, owners));
                if (!res1.IsAcknowledged) return DbResult.Fail();
                var filter_book = Builders<BookDoc>.Filter.Eq(m => m.Id, book.Id);
                var bookDoc = (await context.BookCollection.FindAsync(filter_book)).FirstOrDefaultAsync().Result;
                books.Add(bookDoc.Adapt<BookInfo>());
            }
            var res2 = await context.PersonCollection.UpdateOneAsync(filter_person,
                Builders<PersonDoc>
                .Update
                .Set(r => r.LastUpdatedOn, DateTime.UtcNow.ToLocalTime())
                .Set(r => r.Books, books));
            return res2.IsAcknowledged ? DbResult.Succeed() : DbResult.Fail();
        }

        /// <summary>
        /// Return a list of books
        /// </summary>
        /// <param name="Id">Person id</param>
        /// <param name="SelectedBooks">Selected books to return</param>
        /// <returns>A DbResult structure containing the result of the database operation</returns>
        public async Task<DbResult> ReturnBooksAsync(Guid Id, IEnumerable<BookInfo> SelectedBooks)
        {
            var filter_person = Builders<PersonDoc>.Filter.Eq(m => m.Id, Id);
            var person = (await context.PersonCollection.FindAsync(filter_person)).FirstOrDefaultAsync().Result;
            var books = person.Books;
            foreach (var book in SelectedBooks)
            {
                var res1 = await context.BookCollection.UpdateOneAsync(b => b.Id.Equals(book.Id),
                    Builders<BookDoc>
                    .Update
                    .Set(r => r.LastUpdated, DateTime.UtcNow.ToLocalTime())
                    .Set(r => r.IsAvailable, true));
                if (!res1.IsAcknowledged) return DbResult.Fail();
                foreach (var selected in books.Where(selected => selected.Id.Equals(book.Id)))
                {
                    books.Remove(selected);
                    break;
                }
            }
            var res2 = await context.PersonCollection.UpdateOneAsync(filter_person,
                Builders<PersonDoc>
                .Update
                .Set(r => r.LastUpdatedOn, DateTime.UtcNow.ToLocalTime())
                .Set(r => r.Books, books));
            return res2.IsAcknowledged ? DbResult.Succeed() : DbResult.Fail();
        }
    }
}