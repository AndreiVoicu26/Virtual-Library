using MongoDB.Bson.Serialization.Attributes;
using PersonBook.Core.Info;

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
            Books = null;
            LastUpdatedOn = DateTime.MinValue;
        }

        [BsonId]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public IList<BookInfo>? Books { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime LastUpdatedOn { get; set; }
    }
}
