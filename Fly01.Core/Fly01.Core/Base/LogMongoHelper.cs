using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Authentication;

namespace Fly01.Core.Base
{
    public class LogMongoHelper<T> : IDisposable
    {
        private bool disposed = false;
        #region IDisposable
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                }
            }

            this.disposed = true;
        }
        #endregion

        private string userName
        {
            get { return ConfigurationManager.AppSettings["MongoUserName"]; }
        }

        private string host
        {
            get { return ConfigurationManager.AppSettings["MongoHost"]; }
        }

        private string password
        {
            get { return ConfigurationManager.AppSettings["MongoPassword"]; }
        }

        private string dataBaseName
        {
            get { return ConfigurationManager.AppSettings["MongoDataBaseName"]; }
        }

        private string collectionName
        {
            get { return ConfigurationManager.AppSettings["MongoCollectionNameLog"]; }
        }

        private MongoClient _mongoClient;

        private MongoClient mongoClient
        {
            get
            {
                if (_mongoClient == null)
                {
                    MongoClientSettings settings = new MongoClientSettings()
                    {
                        Server = new MongoServerAddress(host, 10255),
                        UseSsl = true,
                        SslSettings = new SslSettings()
                        {
                            EnabledSslProtocols = SslProtocols.Tls12
                        },
                        Credentials = new List<MongoCredential>()
                        {
                            new MongoCredential("SCRAM-SHA-1", new MongoInternalIdentity(dataBaseName, userName), new PasswordEvidence(password))
                        }
                    };

                    _mongoClient = new MongoClient(settings);
                }

                return _mongoClient;
            }
        }

        public long Count()
        {
            var collection = GetCollection();
            return collection.Count(new BsonDocument());
        }

        public List<T> GetAll()
        {
            try
            {
                var collection = GetCollection();
                return collection.Find<T>(new BsonDocument()).ToList();
            }
            catch (MongoConnectionException)
            {
                return new List<T>();
            }
        }

        public IMongoCollection<T> GetCollection()
        {
            var database = mongoClient.GetDatabase(dataBaseName);
            var collection = database.GetCollection<T>(collectionName);
            return collection;
        }
    }
}