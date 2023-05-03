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
        /// <param name="Name">Person's name</param>        
        /// <returns>A DbResult structure containing the result of the database operation</returns>        
        public async Task<DbResult> AddPersonAsync(string Name)
        {
            var Persons = await GetPersons();
            if (Persons.Any(a => a.Name.Equals(Name)))
            {
                return DbResult.Fail($"A person named '{Name}' already exists.");
            }
            var newDoc = new PersonDoc
            {
                Name = Name,                
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
        /// <param name="Name">Person name</param>
        /// <returns>A DbResult structure containing the result of the database operation</returns>
        public async Task<DbResult> SetPersonNameAsync(Guid Id, string Name)
        {
            var res = await context.PersonCollection.UpdateOneAsync(r => r.Id.Equals(Id),
                Builders<PersonDoc>
                .Update
                .Set(r => r.LastUpdatedOn, DateTime.UtcNow.ToLocalTime())
                .Set(r => r.Name, Name));
            return res.IsAcknowledged ? DbResult.Succeed() : DbResult.Fail();
        }

        /// <summary>
        /// Set the person age
        /// </summary>
        /// <param name="Id">Person id</param>
        /// <param name="Age">Person age</param>        
        /// <returns>A DbResult structure containing the result of the database operation</returns>
        public async Task<DbResult> SetPersonAgeAsync(Guid Id, int Age)
        {
            var res = await context.PersonCollection.UpdateOneAsync(r => r.Id.Equals(Id),
                Builders<PersonDoc>
                .Update
                .Set(r => r.LastUpdatedOn, DateTime.UtcNow.ToLocalTime())
                .Set(r => r.Age, Age));
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