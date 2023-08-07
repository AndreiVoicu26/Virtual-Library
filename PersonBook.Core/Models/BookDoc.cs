using MongoDB.Bson.Serialization.Attributes;
using PersonBook.Core.Info;

namespace PersonBook.Core.Models
{
    [BsonIgnoreExtraElements]
    public class BookDoc
    {
        public const string CollectionName = "Books";

        public BookDoc() {
            Title = string.Empty;
            Author = string.Empty;
            Isbn = string.Empty;
            Year = short.MinValue;
            IsAvailable = true;
            Owners = null;
            LastUpdated = DateTime.MinValue;
        }

        [BsonId]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Isbn { get; set; }
        public short Year { get; set; }
        public bool IsAvailable { get; set; }
        public IList<PersonInfo>? Owners { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime LastUpdated { get; set; }

    }
}
