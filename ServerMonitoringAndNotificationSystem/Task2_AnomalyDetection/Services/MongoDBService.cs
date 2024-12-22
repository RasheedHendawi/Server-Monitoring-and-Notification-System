using MongoDB.Bson;
using MongoDB.Driver;
using Shared.Models;

namespace Task2_AnomalyDetection.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<ServerStatistics> _collection;

        public MongoDBService(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<ServerStatistics>(collectionName);
        }

        public async Task InsertAsync(ServerStatistics stats)
        {
            await _collection.InsertOneAsync(stats);
        }

        public async Task<List<ServerStatistics>> GetRecentAsync(string serverIdentifier, int limit = 10)
        {
            return await _collection.Find(stat => stat.ServerIdentifier == serverIdentifier)
                                     .SortByDescending(stat => stat.Timestamp)
                                     .Limit(limit)
                                     .ToListAsync();
        }
    }
}
