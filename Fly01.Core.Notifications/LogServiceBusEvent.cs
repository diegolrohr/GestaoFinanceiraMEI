using MongoDB.Bson.Serialization.Attributes;

namespace Fly01.Core.Notifications
{
    public class LogServiceBusEvent : RecordBaseMongoDB
    {
        [BsonElement("MessageData")]
        public string MessageData { get; set; }

        [BsonElement("Error")]
        public string Error { get; set; }

        [BsonElement("StackTrace")]
        public string StackTrace { get; set; }

        [BsonElement("Host")]
        public string Host { get; set; }

        [BsonElement("Queue")]
        public string Queue { get; set; }
    }
}