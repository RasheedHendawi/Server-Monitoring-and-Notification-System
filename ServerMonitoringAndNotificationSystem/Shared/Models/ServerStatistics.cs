using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.Models
{
        public class ServerStatistics
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public string Id { get; set; }
            public double MemoryUsage { get; set; } // in MB
            public double AvailableMemory { get; set; } // in MB
            public double CpuUsage { get; set; }
            public DateTime Timestamp { get; set; }
            public string ServerIdentifier { get; set; }
        }
}
