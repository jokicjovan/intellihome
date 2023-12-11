using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;

namespace IntelliHome_Backend.Features.Shared.Influx
{
    public class InfluxRepository : IDisposable
    {
        private readonly InfluxDbConnectionPool _connectionPool;
        private readonly string _bucket;
        private readonly string _database;

        public InfluxRepository(InfluxDbConnectionPool connectionPool, string bucket, string database)
        {
            _connectionPool = connectionPool;
            _bucket = bucket;
            _database = database;
        }
        public async Task WriteToInfluxAsync(string measurement, IDictionary<string, object> fields, IDictionary<string, string> tags)
        {
            using var client = _connectionPool.Get();
            var point = PointData.Measurement(measurement).Timestamp(DateTime.Now, WritePrecision.Ns);

            if (fields != null)
            {
                point = fields.Aggregate(point, (current, field) => current.Field(field.Key, field.Value));
            }

            if (tags != null)
            {
                point = tags.Aggregate(point, (current, tag) => current.Tag(tag.Key, tag.Value));
            }

            using var writeApi = client.GetWriteApi();
            writeApi.WritePoint(point, _database);

        }

        public async Task<IEnumerable<FluxTable>> QueryFromInfluxAsync(string query)
        {
            try
            {
                using var client = _connectionPool.Get();
                var fluxClient = client.GetQueryApi();

                var fluxTablesAsync = await fluxClient.QueryAsync(query, "IntelliHome");
                return fluxTablesAsync;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error querying InfluxDB: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<FluxTable>> GetHistoricalData(string measurement, Guid deviceId, DateTime from, DateTime to)
        {
            var query = $"from(bucket: \"{_bucket}\") " +
                        $"|> range(start: {from:yyyy-MM-ddTHH:mm:ssZ}, stop: {to:yyyy-MM-ddTHH:mm:ssZ}) " +
                        $"|> filter(fn: (r) => r.device_id == \"{deviceId}\" and r._measurement == \"{measurement}\") " +
                        $"|> group(columns: [\"_time\", \"_measurement\", \"device_id\"])";


            return await QueryFromInfluxAsync(query);
        }

        public async Task<FluxTable> GetLastData(string measurement, Guid deviceId)
        {
            var query = $"from(bucket: \"{_bucket}\") " +
                        $"|> range(start: -10d) " +
                        $"|> filter(fn: (r) => r.device_id == \"{deviceId}\" and r._measurement == \"{measurement}\") " +
                        $"|> last()" +
                        $"|> group(columns: [\"_time\", \"_measurement\", \"device_id\"])";

            var result = await QueryFromInfluxAsync(query);

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<FluxTable>> GetLastHourData(Guid deviceId)
        {
            var query = $"from(bucket: \"{_bucket}\") " +
                        $"|> range(start: -1h) " +
                        $"|> filter(fn: (r) => r.device_id == \"{deviceId}\") " +
                        $"|> group(columns: [\"_time\", \"_measurement\", \"device_id\"])";


            return await QueryFromInfluxAsync(query); ;
        }


        public void Dispose()
        {
        }
    }
}
