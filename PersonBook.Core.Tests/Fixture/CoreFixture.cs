using PersonBook.Core.Data;
using MongoDB.Bson;
using MongoDB.Driver;
using PersonBook.Core.Repositories;

namespace PersonBook.Core.Tests.Fixture
{
    public class CoreFixture : IDisposable
    {
        public DataContext DbContext { get; private set; }
        public PersonRepository PersonRepository { get; set; }
        public BookRepository BookRepository { get; set; }
        private readonly string connectionString = "mongodb+srv://abdroot:Test123@cluster0.dusbo.mongodb.net/PersonTest?retryWrites=true&w=majority";

        public CoreFixture()
        {
            // Build database context based on the connection string
            DbContext = new DataContext(connectionString);
            DbContext.PersonCollection.DeleteMany(new BsonDocument());            
            PersonRepository = new PersonRepository(DbContext);
            DbContext.BookCollection.DeleteMany(new BsonDocument());
            BookRepository = new BookRepository(DbContext);
        }

        public void Dispose()
        {    
        }
    }
}
