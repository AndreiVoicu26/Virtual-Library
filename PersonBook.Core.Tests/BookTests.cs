using PersonBook.Core.Repositories;
using PersonBook.Core.Tests.Fixture;

namespace PersonBook.Core.Tests
{
    public class BookTests : IClassFixture<CoreFixture>
    {
        private readonly CoreFixture fixture;

        public BookTests(CoreFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async void AddBookTest()
        {
            var jur = new BookRepository(fixture.DbContext);
            var res1 = await jur.AddBookAsync("Book Title", "Book Author", "Book ISBN", 2000);
            Assert.True(res1.Success);
            var res2 = await jur.GetBookById(res1.Id);
            Assert.True(res2 != null);
        }

        [Fact]
        public async void AddDuplicateBookTest()
        {
            var jur = new BookRepository(fixture.DbContext);
            var res1 = await jur.AddBookAsync("Book-Title", "Book Author", "Book ISBN", 2000);
            var res2 = await jur.AddBookAsync("Book-Title", "Book Author", "Book ISBN", 2000);
            Assert.True(res1.Success);
            Assert.False(res2.Success);
        }

        [Fact]
        public async void SetPersonTitleTest()
        {
            var jur = new BookRepository(fixture.DbContext);
            var res1 = await jur.AddBookAsync("Book-Title-1", "Book Author", "Book ISBN", 2000);
            Assert.True(res1.Success);
            var res2 = await jur.SetBookTitleAsync(res1.Id, "Set-Book-Title-1");
            Assert.True(res2.Success);
            var mi = await jur.GetBookById(res1.Id);
            Assert.Equal("Set-Book-Title-1", mi.Title);
        }

        [Fact]
        public async void SetPersonAuthorTest()
        {
            var jur = new BookRepository(fixture.DbContext);
            var res1 = await jur.AddBookAsync("Book-Title-2", "Book Author", "Book ISBN", 2000);
            Assert.True(res1.Success);
            var res2 = await jur.SetBookAuthorAsync(res1.Id, "Set-Book-Author");
            Assert.True(res2.Success);
            var mi = await jur.GetBookById(res1.Id);
            Assert.Equal("Set-Book-Author", mi.Author);
        }

        [Fact]
        public async void SetPersonIsbnTest()
        {
            var jur = new BookRepository(fixture.DbContext);
            var res1 = await jur.AddBookAsync("Book-Title-3", "Book Author", "Book ISBN", 2000);
            Assert.True(res1.Success);
            var res2 = await jur.SetBookIsbnAsync(res1.Id, "Set-Book-Isbn");
            Assert.True(res2.Success);
            var mi = await jur.GetBookById(res1.Id);
            Assert.Equal("Set-Book-Isbn", mi.Isbn);
        }

        [Fact]
        public async void SetPersonYearTest()
        {
            var jur = new BookRepository(fixture.DbContext);
            var res1 = await jur.AddBookAsync("Book-Title-4", "Book Author", "Book ISBN", 2000);
            Assert.True(res1.Success);
            var res2 = await jur.SetBookYearAsync(res1.Id, 2001);
            Assert.True(res2.Success);
            var mi = await jur.GetBookById(res1.Id);
            Assert.Equal(2001, mi.Year);
        }

        [Fact]
        public async void RemoveBookTest()
        {
            var jur = new BookRepository(fixture.DbContext);
            var res1 = await jur.AddBookAsync("Book-Title-5", "Book Author", "Book ISBN", 2000);
            var res2 = await jur.RemoveBookAsync(res1.Id);
            Assert.True(res1.Success);
            Assert.True(res2.Success);
        }
    }
}
