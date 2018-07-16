using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;

namespace Fly01.Core.Entities.Domains.NoSQL
{
    public class RecordBaseMongoDB
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid Id { get; set; }

        [BsonElement("EventDate")]
        public DateTime EventDate { get; set; }

        [BsonElement("PlatformId")]
        public string PlatformId { get; set; }

        public RecordBaseMongoDB()
        {
            Id = Guid.NewGuid();
            EventDate = DateTime.Now;
        }
    }
}