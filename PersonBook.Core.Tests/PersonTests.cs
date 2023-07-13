using PersonBook.Core.Tests.Fixture;
using PersonBook.Core;
using PersonBook.Core.Data;

namespace PersonBook.Core.Tests
{
    public class PersonTests : IClassFixture<CoreFixture>
    {
        private readonly CoreFixture fixture;

        public PersonTests(CoreFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async void AddPersonTest()
        {
            var jur = new PersonRepository(fixture.DbContext);
            var res1 = await jur.AddPersonAsync("Person-FirstName", "Person-LastName");
            Assert.True(res1.Success);
            var res2 = await jur.GetPersonById(res1.Id);
            Assert.True(res2 != null);
        }

        [Fact]
        public async void AddDuplicatePersonTest()
        {
            var jur = new PersonRepository(fixture.DbContext);
            var res1 = await jur.AddPersonAsync("Person FirstName", "Person LastName");
            var res2 = await jur.AddPersonAsync("Person FirstName", "Person LastName");
            Assert.True(res1.Success);
            Assert.False(res2.Success);
        }

        [Fact]
        public async void SetPersonFirstNameTest()
        {
            var jur = new PersonRepository(fixture.DbContext);
            var res1 = await jur.AddPersonAsync("Person-Set-FirstName", "Person-Set-LastName");
            Assert.True(res1.Success);
            var res2 = await jur.SetPersonFirstNameAsync(res1.Id, "Person FirstName");
            Assert.True(res2.Success);
            var mi = await jur.GetPersonById(res1.Id);
            Assert.Equal("Person FirstName", mi.FirstName);
        }

        [Fact]
        public async void SetPersonLastNameTest()
        {
            var jur = new PersonRepository(fixture.DbContext);
            var res1 = await jur.AddPersonAsync("Person Set FirstName", "Person Set LastName");
            Assert.True(res1.Success);
            var res2 = await jur.SetPersonLastNameAsync(res1.Id, "Person LastName");
            Assert.True(res2.Success);
            var mi = await jur.GetPersonById(res1.Id);
            Assert.Equal("Person LastName", mi.LastName);
        }

        [Fact]
        public async void SetPersonDateOfBirthTest()
        {
            var jur = new PersonRepository(fixture.DbContext);
            var res1 = await jur.AddPersonAsync("Person-Set-Description-FirstName", "Person-Set-Description-LastName");
            Assert.True(res1.Success);
            var res2 = await jur.SetPersonDateOfBirthAsync(res1.Id, DateOnly.FromDateTime(DateTime.UtcNow));
            Assert.True(res2.Success);
            var mi = await jur.GetPersonById(res1.Id);
            Assert.Equal(DateOnly.FromDateTime(DateTime.UtcNow), mi.DateOfBirth);
        }
        
        [Fact]
        public async void RemovePersonsTest()
        {
            var jur = new PersonRepository(fixture.DbContext);
            var res1 = await jur.AddPersonAsync("Person-Del", "Person-Del");
            var res2 = await jur.RemovePersonAsync(res1.Id);
            Assert.True(res1.Success);
            Assert.True(res2.Success);
        }
    }
}