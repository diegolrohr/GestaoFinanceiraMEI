using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Fly01.Core.Entities.Domains.NoSQL
{
    public class LogIntegracaoRabbitMQ
    {
        [BsonElement("DataInclusao")]
        public DateTime DataInclusao { get; set; }
        [BsonElement("Mensagem")]
        public string Mensagem { get; set; }
        [BsonElement("RoutingKey")]
        public string RoutingKey { get; set; }
        [BsonElement("QueueName")]
        public string QueueName { get; set; }
        [BsonElement("HostName")]
        public string HostName { get; set; }
        [BsonElement("AppId")]
        public string AppId { get; set; }
    }
}