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
            var res1 = await jur.AddPersonAsync("Person-Add");
            Assert.True(res1.Success);
            var res2 = await jur.GetPersonById(res1.Id);
            Assert.True(res2 != null);
        }

        [Fact]
        public async void AddDuplicatePersonTest()
        {
            var jur = new PersonRepository(fixture.DbContext);
            var res1 = await jur.AddPersonAsync("Person-DuP");
            var res2 = await jur.AddPersonAsync("Person-DuP");
            Assert.True(res1.Success);
            Assert.False(res2.Success);
        }

        [Fact]
        public async void SetPersonNameTest()
        {
            var jur = new PersonRepository(fixture.DbContext);
            var res1 = await jur.AddPersonAsync("Person-Set-Name");
            Assert.True(res1.Success);
            var res2 = await jur.SetPersonNameAsync(res1.Id, "Person Name");
            Assert.True(res2.Success);
            var mi = await jur.GetPersonById(res1.Id);
            Assert.Equal("Person Name", mi.Name);
        }

        [Fact]
        public async void SetPersonAgeTest()
        {
            var jur = new PersonRepository(fixture.DbContext);
            var res1 = await jur.AddPersonAsync("Person-Set-Description");
            Assert.True(res1.Success);
            var res2 = await jur.SetPersonAgeAsync(res1.Id, 22);
            Assert.True(res2.Success);
            var mi = await jur.GetPersonById(res1.Id);
            Assert.Equal(22, mi.Age);
        }
        
        [Fact]
        public async void RemovePersonsTest()
        {
            var jur = new PersonRepository(fixture.DbContext);
            var res1 = await jur.AddPersonAsync("Person-Del");
            var res2 = await jur.RemovePersonAsync(res1.Id);
            Assert.True(res1.Success);
            Assert.True(res2.Success);
        }
    }
}