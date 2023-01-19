using Amazon.Core.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Amazon.Services
{
    public class ResponseCashService : IResponseCashService
    {
        private readonly IDatabase _database;

        public ResponseCashService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task CashResponseAsync(string cashKey, object response, TimeSpan timeToLive)
        {
            if (response == null) return;

            var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var serializedResponse = JsonSerializer.Serialize(response , options);
            await _database.StringSetAsync(cashKey, serializedResponse, timeToLive);
        }

        public async Task<string> GetCashedResponseAsync(string cashKey)
        {
            var cashResponse = await _database.StringGetAsync(cashKey);

            if (cashResponse.IsNullOrEmpty) return null;
            return cashResponse;
        }
    }
}
