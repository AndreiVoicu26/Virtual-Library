using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonBook.Core.Models
{
    [BsonIgnoreExtraElements]
    public class PersonDoc
    {
        public const string CollectionName = "Persons";

        public PersonDoc() {
            FirstName = string.Empty;
            LastName = string.Empty;
            DateOfBirth = DateOnly.MinValue;
            LastUpdatedOn = DateTime.MinValue;
        }

        [BsonId]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime LastUpdatedOn { get; set; }
    }
}
