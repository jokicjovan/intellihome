using InfluxDB.Client;
using Microsoft.Extensions.ObjectPool;

namespace IntelliHome_Backend.Features.Shared.Influx
{
    public class InfluxDbConnectionPool : DefaultObjectPool<InfluxDBClient>
    {
        public InfluxDbConnectionPool(ObjectPoolProvider poolProvider, string url, string token, string organization, string bucket)
            : base(new InfluxDbConnectionPooledObjectPolicy(url, token, organization, bucket), Environment.ProcessorCount * 2)
        {
        }
    }
}
