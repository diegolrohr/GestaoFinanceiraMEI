using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;

namespace Fly01.Core.Notifications
{
    public class LogServiceBusEvent
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid Id { get; set; }

        [BsonElement("EventDate")]
        public DateTime EventDate { get; set; }

        [BsonElement("MessageData")]
        public string MessageData { get; set; }

        [BsonElement("Error")]
        public string Error { get; set; }

        [BsonElement("Host")]
        public string Host { get; set; }

        [BsonElement("Queue")]
        public string Queue { get; set; }

        public LogServiceBusEvent()
        {
            Id = Guid.NewGuid();
            EventDate = DateTime.Now;
        }
    }
}
