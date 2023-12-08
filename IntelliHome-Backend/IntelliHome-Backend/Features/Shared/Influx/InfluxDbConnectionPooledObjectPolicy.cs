using InfluxDB.Client;
using Microsoft.Extensions.ObjectPool;

namespace IntelliHome_Backend.Features.Shared.Influx
{
    public class InfluxDbConnectionPooledObjectPolicy : IPooledObjectPolicy<InfluxDBClient>
    {
        private readonly string _url;
        private readonly string _token;
        private readonly string _organization;
        private readonly string _bucket;

        public InfluxDbConnectionPooledObjectPolicy(string url, string token, string organization, string bucket)
        {
            _url = url;
            _token = token;
            _organization = organization;
            _bucket = bucket;
        }

        public InfluxDBClient Create()
        {
            var clientOptions = new InfluxDBClientOptions.Builder()
                .Url(_url)
                .AuthenticateToken(_token.ToCharArray())
                .Org(_organization)
                .Bucket(_bucket)
                .Build();

            return new InfluxDBClient(clientOptions);
        }

        public bool Return(InfluxDBClient obj)
        {
            return true; // Object can always be returned to the pool
        }

        public void OnReturn(InfluxDBClient obj)
        {
            // Reset the state of the object if needed
        }
    }
}
