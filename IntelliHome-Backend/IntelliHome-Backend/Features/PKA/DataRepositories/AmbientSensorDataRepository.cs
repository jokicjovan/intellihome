using Data.Context;
using InfluxDB.Client.Core.Flux.Domain;
using IntelliHome_Backend.Features.PKA.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.PKA.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace IntelliHome_Backend.Features.PKA.DataRepositories
{
    public class AmbientSensorDataRepository : IAmbientSensorDataRepository
    {
        private readonly InfluxDbContext _context;

        public AmbientSensorDataRepository(InfluxDbContext context)
        {
            _context = context;
        }

        public List<AmbientSensorHistoricalDataDTO> GetHistoricalData(Guid id, DateTime from, DateTime to)
        {
            var result = _context.GetHistoricalData(id, from, to).Result;

            List<AmbientSensorHistoricalDataDTO> list = new List<AmbientSensorHistoricalDataDTO>();

            foreach (var table in result)
            {
               list.Add(ConvertToAmbientSensorData(table));
            }

            return list;
        }


        public void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _context.WriteToInfluxAsync("ambient_sensor", fields, tags);
        }

        public AmbientSensorData GetLastData(Guid id)
        {
           
            var table = _context.GetLastData(id).Result;

            AmbientSensorHistoricalDataDTO data = ConvertToAmbientSensorData(table);

            return new AmbientSensorData
            {
                Temperature = data.Temperature,
                Humidity = data.Humidity,
            };
        }


        private AmbientSensorHistoricalDataDTO ConvertToAmbientSensorData(FluxTable table)
        {
            var rows = table.Records;
            DateTime timestamp = DateTime.Parse(rows[0].GetValueByKey("_time").ToString());
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            timestamp = TimeZoneInfo.ConvertTime(timestamp, localTimeZone);
            double temperature = 0;
            double humidity = 0;
            if (rows[0].Row.Contains("temperature"))
            {
                temperature = Convert.ToDouble(rows[0].GetValueByKey("_value"));
                humidity = Convert.ToDouble(rows[1].GetValueByKey("_value"));
            }
            else
            {
                temperature = Convert.ToDouble(rows[1].GetValueByKey("_value"));
                humidity = Convert.ToDouble(rows[0].GetValueByKey("_value"));
            }

            AmbientSensorHistoricalDataDTO dto = new AmbientSensorHistoricalDataDTO
            {
                Timestamp = timestamp,
                Temperature = temperature,
                Humidity = humidity
            };

            return dto;
        }
    }

}
