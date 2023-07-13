using PersonBook.Core.Data;
using PersonBook.Core.Models;
using Mapster;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics.Tracing;
using System.Runtime.Versioning;
using static System.Net.Mime.MediaTypeNames;

namespace PersonBook.Core
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
    }
}