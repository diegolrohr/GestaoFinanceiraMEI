using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Authentication;

namespace Fly01.Core.Helpers
{
    public class LogMongoHelper<T> : IDisposable
    {
        private string Host { get; set; }
        private string UserName { get; set; }
        private string Password { get; set; }
        private string DataBaseName { get; set; }
        private string Port { get; set; }

        private MongoClient _mongoClient;
        private MongoClient MongoClient
        {
            get
            {
                if (_mongoClient == null)
                {
                    MongoClientSettings settings = new MongoClientSettings()
                    {
                        Server = new MongoServerAddress(Host, Convert.ToInt32(Port)),
                        UseSsl = false,
                        //SslSettings = new SslSettings()
                        //{
                        //    EnabledSslProtocols = SslProtocols.Tls12
                        //},
                        Credentials = new List<MongoCredential>()
                        {
                            new MongoCredential("SCRAM-SHA-1", new MongoInternalIdentity(UserName, UserName), new PasswordEvidence(Password))
                        }
                    };

                    _mongoClient = new MongoClient(settings);
                }

                return _mongoClient;
            }
        }

        public LogMongoHelper(string dbName)
            : this(ConfigurationManager.AppSettings["MongoHost"], 
                  ConfigurationManager.AppSettings["MongoUserName"], 
                  ConfigurationManager.AppSettings["MongoPassword"],
                  ConfigurationManager.AppSettings["MongoPort"], dbName)
        { }

        public LogMongoHelper(string host, string userName, string password, string port, string databaseName)
        {
            Host = host;
            UserName = userName;
            Password = password;
            DataBaseName = databaseName;
            Port = string.IsNullOrEmpty(port) ? "10255" : port;
    }

        public void Dispose() => GC.SuppressFinalize(this);

        public long Count(string collectionName)
        {
            var collection = GetCollection(collectionName);
            return collection.Count(new BsonDocument());
        }

        public List<T> GetAll(string collectionName, FilterDefinition<T> filter)
        {
            try
            {
                var collection = GetCollection(collectionName);
                return collection.Find(filter).ToList();
            }
            catch (MongoConnectionException)
            {
                return new List<T>();
            }
        }

        public IMongoCollection<T> GetCollection(string collectionName)
        {
            var database = MongoClient.GetDatabase(DataBaseName);
            var collection = database.GetCollection<T>(collectionName);
            return collection;
        }

        public void CreateIndex(string collectionName, string fieldName)
        {
            var collection = this.GetCollection(collectionName);
            var indexOptions = new CreateIndexOptions
            {
                Collation = new Collation("pt", strength: CollationStrength.Secondary)
            };

            collection.Indexes.CreateOne(fieldName, indexOptions);
        }
    }
}