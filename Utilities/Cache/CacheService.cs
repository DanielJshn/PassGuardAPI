namespace apief
{
    using StackExchange.Redis;
    using System.Text.Json;

    public class CacheService : ICacheService
    {
        private readonly IDatabase _cacheDb;
        private readonly IConnectionMultiplexer _redis;

        public CacheService(IConnectionMultiplexer redis)
        {
            _cacheDb = redis.GetDatabase();
            _redis = redis;
        }

        public async Task<string> GetCacheValueAsync(string key)
        {
            var db = _redis.GetDatabase();
            var value = await db.StringGetAsync(key);

            if (value.IsNullOrEmpty)
            {
                Console.WriteLine($"Key {key} not found in cache.");
                return null;
            }

            Console.WriteLine($"Key {key} found in cache with value: {value}");
            return value;
        }


        public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
        {
            var jsonData = JsonSerializer.Serialize(value);
            await _cacheDb.StringSetAsync(key, jsonData, expiration);
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _cacheDb.StringGetAsync(key);
            return value.HasValue ? JsonSerializer.Deserialize<T>(value) : default;
        }

        public async Task RemoveAsync(string key)
        {
            await _cacheDb.KeyDeleteAsync(key);
        }
    }

}