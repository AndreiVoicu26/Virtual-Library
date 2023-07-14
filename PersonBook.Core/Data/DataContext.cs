using PersonBook.Core.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonBook.Core.Data
{
    public class DataContext
    {
        public IMongoDatabase Context { get; private set; }        
        public IMongoCollection<PersonDoc> PersonCollection { get; set; }
        public IMongoCollection<BookDoc> BookCollection { get; set; }

        private string ConnectionString { get; set; }
        private string DatabaseName { get; set; }
        
        public DataContext(string ConnectionString)
        {
            this.ConnectionString = ConnectionString;
            DatabaseName = MongoUrl.Create(ConnectionString).DatabaseName;
            var client = new MongoClient(ConnectionString);
            if (client != null)
                Context = client.GetDatabase(DatabaseName);
                        
            PersonCollection = Context.GetCollection<PersonDoc>(PersonDoc.CollectionName); 
            BookCollection = Context.GetCollection<BookDoc>(BookDoc.CollectionName);
        }

        public void DropDatabase()
        {
            var client = new MongoClient(ConnectionString);
            client.DropDatabase(DatabaseName);
        }
    }
}
