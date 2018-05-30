using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Authentication;

namespace Fly01.Core.Notifications
{
    public class LogMongoHelper<T> : IDisposable
    {
        private string Host { get; set; }
        private string UserName { get; set; }
        private string Password { get; set; }
        private string DataBaseName { get; set; }

        public LogMongoHelper(string dbName)
            : this(ConfigurationManager.AppSettings["MongoHost"], ConfigurationManager.AppSettings["MongoUserName"], ConfigurationManager.AppSettings["MongoPassword"], dbName) { }

        public LogMongoHelper(string host, string userName, string password, string databaseName)
        {
            Host = host;
            UserName = userName;
            Password = password;
            DataBaseName = databaseName;
        }

        public void Dispose() => GC.SuppressFinalize(this);

        private MongoClient _mongoClient;
        private MongoClient MongoClient
        {
            get
            {
                if (_mongoClient == null)
                {
                    MongoClientSettings settings = new MongoClientSettings()
                    {
                        Server = new MongoServerAddress(Host, 10255),
                        UseSsl = true,
                        SslSettings = new SslSettings()
                        {
                            EnabledSslProtocols = SslProtocols.Tls12
                        },
                        Credentials = new List<MongoCredential>()
                        {
                            new MongoCredential("SCRAM-SHA-1", new MongoInternalIdentity(DataBaseName, UserName), new PasswordEvidence(Password))
                        }
                    };

                    _mongoClient = new MongoClient(settings);
                }

                return _mongoClient;
            }
        }

        public long Count(string collectionName)
        {
            var collection = GetCollection(collectionName);
            return collection.Count(new BsonDocument());
        }

        public List<T> GetAll(string collectionName)
        {
            try
            {
                var collection = GetCollection(collectionName);
                return collection.Find<T>(new BsonDocument()).ToList();
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
    }
}