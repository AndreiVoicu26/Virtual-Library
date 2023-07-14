using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            LastUpdated = DateTime.MinValue;
        }

        [BsonId]
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Isbn { get; set; }
        public short Year { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime LastUpdated { get; set; }

    }
}
