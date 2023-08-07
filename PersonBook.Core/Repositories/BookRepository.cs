using Mapster;
using MongoDB.Bson;
using MongoDB.Driver;
using PersonBook.Core.Data;
using PersonBook.Core.Info;
using PersonBook.Core.Models;

namespace PersonBook.Core.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly DataContext context;

        public BookRepository(DataContext context)
        {
            this.context = context;
        }
        /// <summary>
        /// Create a new book
        /// </summary>
        /// <param name="Title">Book title</param> 
        /// <param name="Author">Book author</param>
        /// <param name="Isbn">Book ISBN</param>
        /// <param name="Year">Book Year</param>
        /// <returns>A DbResult structure containing the result of the database operation</returns>     
        public async Task<DbResult> AddBookAsync(string Title, string Author, string Isbn, short Year)
        {
            var Books = await GetBooks();
            if (Books.Any(b => b.Title.Equals(Title)))
            {
                return DbResult.Fail($"A book named '{Title}' already exists");
            }
            var newDoc = new BookDoc
            {
                Title = Title,
                Author = Author,
                Isbn = Isbn,
                Year = Year,
                IsAvailable = true,
                LastUpdated = DateTime.UtcNow.ToLocalTime()
            };
            try
            {
                await context.BookCollection.InsertOneAsync(newDoc);
                return DbResult.Succeed(newDoc.Id);
            }
            catch (Exception ex)
            {
                return DbResult.Fail(ex);
            }
        }

        /// <summary>
        /// Set the book title
        /// </summary>
        /// <param name="Id">Book id</param>
        /// <param name="Title">Book title</param>
        /// <returns>A DbResult structure containing the result of the database operation</returns>
        public async Task<DbResult> SetBookTitleAsync(Guid Id, string Title)
        {
            var Books = await GetBooks();
            if (Books.Any(b => b.Title.Equals(Title)))
            {
                return DbResult.Fail($"A book named '{Title}' already exists");
            }

            var res = await context.BookCollection.UpdateOneAsync(r => r.Id.Equals(Id),
                Builders<BookDoc>
                .Update
                .Set(r => r.LastUpdated, DateTime.UtcNow.ToLocalTime())
                .Set(r => r.Title, Title));
            return res.IsAcknowledged ? DbResult.Succeed() : DbResult.Fail();
        }

        /// <summary>
        /// Set the book author
        /// </summary>
        /// <param name="Id">Book id</param>
        /// <param name="Author">Book author</param>
        /// <returns>A DbResult structure containing the result of the database operation</returns>
        public async Task<DbResult> SetBookAuthorAsync(Guid Id, string Author)
        {
            var res = await context.BookCollection.UpdateOneAsync(r => r.Id.Equals(Id),
                Builders<BookDoc>
                .Update
                .Set(r => r.LastUpdated, DateTime.UtcNow.ToLocalTime())
                .Set(r => r.Author, Author));
            return res.IsAcknowledged ? DbResult.Succeed() : DbResult.Fail();
        }

        /// <summary>
        /// Set the book ISBN
        /// </summary>
        /// <param name="Id">Book id</param>
        /// <param name="Isbn">Book ISBN</param>
        /// <returns>A DbResult structure containing the result of the database operation</returns>
        public async Task<DbResult> SetBookIsbnAsync(Guid Id, string Isbn)
        {
            var res = await context.BookCollection.UpdateOneAsync(r => r.Id.Equals(Id),
                Builders<BookDoc>
                .Update
                .Set(r => r.LastUpdated, DateTime.UtcNow.ToLocalTime())
                .Set(r => r.Isbn, Isbn));
            return res.IsAcknowledged ? DbResult.Succeed() : DbResult.Fail();
        }

        /// <summary>
        /// Set the book year
        /// </summary>
        /// <param name="Id">Book id</param>
        /// <param name="Year">Book year</param>
        /// <returns>A DbResult structure containing the result of the database operation</returns>
        public async Task<DbResult> SetBookYearAsync(Guid Id, short Year)
        {
            var res = await context.BookCollection.UpdateOneAsync(r => r.Id.Equals(Id),
                Builders<BookDoc>
                .Update
                .Set(r => r.LastUpdated, DateTime.UtcNow.ToLocalTime())
                .Set(r => r.Year, Year));
            return res.IsAcknowledged ? DbResult.Succeed() : DbResult.Fail();
        }

        /// <summary>
        /// Get all registered books.
        /// </summary>
        /// <returns>An enumeration of BookInfo objects</returns>
        public async Task<IEnumerable<BookInfo>> GetBooks()
        {
            var dbObjs = await context.BookCollection.FindAsync(new BsonDocument());
            return dbObjs.ToList().Adapt<IEnumerable<BookInfo>>();
        }

        /// <summary>
        /// Get book by id
        /// </summary>
        /// <param name="Id">Book id</param>
        /// <returns>A BookInfo object</returns>
        public async Task<BookInfo> GetBookById(Guid Id)
        {
            var filter = Builders<BookDoc>.Filter.Eq(m => m.Id, Id);
            var dbObjs = (await context.BookCollection.FindAsync(filter)).FirstOrDefaultAsync().Result;
            return dbObjs.Adapt<BookInfo>();
        }

        /// <summary>
        /// Remove a specified book
        /// </summary>
        /// <param name="Id">Book id</param>
        /// <returns>A DbResult structure containing the result of the database operation</returns>
        public async Task<DbResult> RemoveBookAsync(Guid Id)
        {
            var filter = Builders<BookDoc>.Filter.Eq(m => m.Id, Id);
            var book = (await context.BookCollection.FindAsync(filter)).FirstOrDefaultAsync().Result;
            if (!book.IsAvailable)
            {
                return DbResult.Fail($"The book '{book.Title} by {book.Author}' is borrowed by someone and cannot be removed.");
            }
            try
            {
                await context.BookCollection.DeleteOneAsync(s => s.Id.Equals(Id));
                return DbResult.Succeed();
            }
            catch (Exception ex)
            {
                return DbResult.Fail(ex);
            }
        }

        /// <summary>
        /// Get all available books
        /// </summary>
        /// <returns>An enumeration of BookInfo objects</returns>
        public async Task<IEnumerable<BookInfo>> GetAvailableBooks()
        {
            var filter = Builders<BookDoc>.Filter.Eq(m => m.IsAvailable, true);
            var books = await context.BookCollection.FindAsync(filter);
            return books.ToList().Adapt<IEnumerable<BookInfo>>();
        }


        public async Task<IEnumerable<PersonInfo>> GetOwnersOfBook(Guid Id)
        {
            var filter = Builders<BookDoc>.Filter.Eq(m => m.Id, Id);
            var book = (await context.BookCollection.FindAsync(filter)).FirstOrDefaultAsync().Result;
            var owners = book.Owners;
            return owners == null ? Enumerable.Empty<PersonInfo>() : owners.ToList().Adapt<IEnumerable<PersonInfo>>();
        }
    }
}
