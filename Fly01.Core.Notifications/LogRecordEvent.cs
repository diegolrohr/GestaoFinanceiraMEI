using MongoDB.Bson.Serialization.Attributes;

namespace Fly01.Core.Notifications
{
    public class LogRecordEvent : RecordBaseMongoDB
    {
        [BsonElement("Username")]
        public string Username { get; set; }

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
    }
}