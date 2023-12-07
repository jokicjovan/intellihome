using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;
using Microsoft.Extensions.Configuration;

namespace Data.Context
{
    public class InfluxRepository : IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly InfluxDBClient _influxDbClient;
        public readonly string _database;
        private readonly string _url = "http://localhost:8086";
        private readonly string _token;
        private readonly string _organization = "IntelliHome";
        private readonly string _bucket = "intellihome_influx";

        public InfluxRepository(IConfiguration configuration)
        {
            _configuration = configuration;

            string tokenFilePath = "InfluxDBToken.txt";

            if (System.IO.File.Exists(tokenFilePath))
            {
                try
                {
                    _token = System.IO.File.ReadAllText(tokenFilePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading token file: {ex.Message}");
                    throw;
                }

                InfluxDBClientOptions options = InfluxDBClientOptions.Builder
                    .CreateNew()
                    .Url(_url)
                    .AuthenticateToken(_token.ToCharArray())
                    .Org(_organization)
                    .Bucket(_bucket)
                    .Build();

                _influxDbClient = InfluxDBClientFactory.Create(options);
            }
            else
            {
                Console.WriteLine("Token file does not exist.");
                throw new InvalidOperationException("Token file does not exist.");
            }
        }



        public async Task WriteToInfluxAsync(string measurement, IDictionary<string, object> fields, IDictionary<string, string> tags = null)
        {
            var point = PointData.Measurement(measurement).Timestamp(DateTime.Now, WritePrecision.Ns);
            
            if (fields != null)
            {
                foreach (var field in fields)
                {
                    point = point.Field(field.Key, field.Value);
                }
            }

            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    point = point.Tag(tag.Key, tag.Value);
                }
            }

            using var writeApi = _influxDbClient.GetWriteApi();
            writeApi.WritePoint(point, _database);

        }




        public async Task<IEnumerable<FluxTable>> QueryFromInfluxAsync(string query)
        {
            try
            {
                var fluxClient = _influxDbClient.GetQueryApi();

                var fluxTablesAsync = await fluxClient.QueryAsync(query, "IntelliHome");
                return fluxTablesAsync;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error querying InfluxDB: {ex.Message}");
                throw; 
            }
        }

        public async Task<IEnumerable<FluxTable>> GetHistoricalData(Guid deviceId, DateTime from, DateTime to)
        {
            var query = $"from(bucket: \"{_bucket}\") " +
                        $"|> range(start: {from:yyyy-MM-ddTHH:mm:ssZ}, stop: {to:yyyy-MM-ddTHH:mm:ssZ}) " +
                        $"|> filter(fn: (r) => r.deviceId == \"{deviceId}\") " +
                        $"|> group(columns: [\"_time\", \"_measurement\", \"deviceId\"])";


            return await QueryFromInfluxAsync(query);
        }

        public async Task<FluxTable> GetLastData(Guid deviceId)
        {
            var query = $"from(bucket: \"{_bucket}\") " +
                        $"|> range(start: -1d) " +
                        $"|> filter(fn: (r) => r.deviceId == \"{deviceId}\") " +
                        $"|> last()" +
                        $"|> group(columns: [\"_time\", \"_measurement\", \"deviceId\"])";

            var result = await QueryFromInfluxAsync(query);

            return result.FirstOrDefault();
        }


        public void Dispose()
        {
            _influxDbClient.Dispose();
        }
    }
}
