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

        public async Task<IEnumerable<FluxTable>> GetHistoricalAvailability(Guid deviceId, string h)
        {
            string aggregation;
            string unit;
            string multiplier;
            string dorh;

            if (h is not ("7d" or "30d"))
            {
                aggregation = "1h";
                unit = "1m";
                multiplier = "60";
                dorh = "h";
            }
            else
            {
                aggregation = "1d";
                unit = "1h";
                multiplier = "24";
                dorh = "d";
            }

            var query = $"import \"interpolate\"" +
                         $"from(bucket: \"{_bucket}\")" +
                         $"|> range(start: -{h})" +
                         $"|> filter(fn: (r) => r[\"_measurement\"] == \"availability\")" +
                         $"|> filter(fn: (r) => r[\"_field\"] == \"isConnected\")" +
                         $"|> filter(fn: (r) => r[\"deviceId\"] == \"{deviceId}\")" +
                         $"|> map(fn: (r) => ({{" +
                         $"      r with" + 
                         $"      _value: if exists r._value then float(v: r._value) else 0.0" +
                         $"  }}))  " +
                         $"|> interpolate.linear(every: 1m)" +
                         $"|> map(fn: (r) => ({{" +
                         $"      r with" +
                         $"      _value: if r._value > 0.5 then 1.0 else 0.0" +
                         $"  }}))" +
                         $"|> window(every: {aggregation}, createEmpty: true)" +
                         $"|> stateDuration(fn: (r) => r._value == 1, unit: {unit})" +
                         $"|> last()" +
                         $"|> map(fn: (r) => ({{" +
                         $"      time: r._time," +
                         $"      duration: r.stateDuration + 1," +
                         $"      percentage: (r.stateDuration + 1) * 100 / {multiplier}," +
                         $"      units: \"{dorh}\"" +
                         $"  }}))" +
                         $"|> group(columns: [\"time\"])";



            return await QueryFromInfluxAsync(query);
        
        }

        public async Task<IEnumerable<FluxTable>> GetHistoricalData(string measurement, Guid deviceId, DateTime from, DateTime to)
        {
            if (to == from)
            {
                to = to.AddSeconds(1);
            }
            var query = $"from(bucket: \"{_bucket}\") " +
                        $"|> range(start: {from:yyyy-MM-ddTHH:mm:ssZ}, stop: {to:yyyy-MM-ddTHH:mm:ssZ}) " +
                        $"|> filter(fn: (r) => r.deviceId == \"{deviceId}\" and r._measurement == \"{measurement}\") " +
                        $"|> group(columns: [\"_time\", \"_measurement\", \"deviceId\"])";


            return await QueryFromInfluxAsync(query);
        }

        public async Task<FluxTable> GetLastData(string measurement, Guid deviceId)
        {
            var query = $"from(bucket: \"{_bucket}\") " +
                        $"|> range(start: -1d) " +
                        $"|> filter(fn: (r) => r.deviceId == \"{deviceId}\" and r._measurement == \"{measurement}\") " +
                        $"|> last()" +
                        $"|> group(columns: [\"_time\", \"_measurement\", \"deviceId\"])";

            var result = await QueryFromInfluxAsync(query);

            return result.LastOrDefault();
        }

        public async Task<IEnumerable<FluxTable>> GetLastHourData(Guid deviceId)
        {
            var query = $"from(bucket: \"{_bucket}\") " +
                        $"|> range(start: -1h) " +
                        $"|> filter(fn: (r) => r.deviceId == \"{deviceId}\") " +
                        $"|> group(columns: [\"_time\", \"_measurement\", \"deviceId\"])";


            return await QueryFromInfluxAsync(query); ;
        }


        public void Dispose()
        {
        }
    }
}
