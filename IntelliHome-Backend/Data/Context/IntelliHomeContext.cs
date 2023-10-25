using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using InfluxDB.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;

namespace Data.Context
{
    public class PostgreSqlDbContext : DbContext
    {
        public readonly IConfiguration _configuration;
        

        public PostgreSqlDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(_configuration.GetConnectionString("IntelliHomeSQLDatabase"));
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }


    public class InfluxDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly InfluxDBClient _influxDbClient;

        public InfluxDbContext(IConfiguration configuration)
        {
            _configuration = configuration;

            string token = System.IO.File.ReadAllText("InfluxDBToken.txts");
            _influxDbClient = InfluxDBClientFactory.Create(_configuration.GetConnectionString("IntelliHomeInfluxDatabase"), token);
        }

    }

}
