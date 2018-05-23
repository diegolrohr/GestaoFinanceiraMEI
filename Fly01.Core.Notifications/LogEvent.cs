using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;

namespace Fly01.Core.Notifications
{
    public class LogRecordEvent
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid Id { get; set; }

        [BsonElement("PlatformId")]
        public string PlatformId { get; set; }

        [BsonElement("Username")]
        public string Username { get; set; }

        [BsonElement("EventDate")]
        public DateTime EventDate { get; set; }

        [BsonElement("EventType")]
        public string EventType { get; set; }

        [BsonElement("TableName")]
        public string TableName { get; set; }

        [BsonElement("RecordId")]
        public string RecordId { get; set; }

        [BsonElement("OriginalValues")]
        public dynamic OriginalValues { get; set; }

        [BsonElement("NewValues")]
        public dynamic NewValues { get; set; }

        public LogRecordEvent()
        {
            Id = Guid.NewGuid();
            EventDate = DateTime.Now;
        }
    }
}