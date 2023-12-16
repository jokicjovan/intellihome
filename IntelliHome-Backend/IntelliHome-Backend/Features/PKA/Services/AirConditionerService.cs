﻿using Data.Models.PKA;
using Data.Models.Shared;
using Data.Models.SPU;
using IntelliHome_Backend.Features.PKA.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.PKA.Handlers;
using IntelliHome_Backend.Features.PKA.Handlers.Interfaces;
using IntelliHome_Backend.Features.PKA.Repositories;
using IntelliHome_Backend.Features.PKA.Repositories.Interfaces;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.Exceptions;
using System.Collections;

namespace IntelliHome_Backend.Features.PKA.Services
{
    public class AirConditionerService : IAirConditionerService
    {
        private readonly IAirConditionerRepository _airConditionerRepository;
        private readonly IAirConditionerHandler _airConditionerHandler;
        private readonly IAirConditionerDataRepository _airConditionerDataRepository;
        private readonly IAirConditionerWorkRepository _airConditionerWorkRepository;

        public AirConditionerService(IAirConditionerRepository airConditionerRepository,IAirConditionerWorkRepository airConditionerWorkRepository, IAirConditionerHandler airConditionerHandler,IAirConditionerDataRepository airConditionerDataRepository)
        {
            _airConditionerRepository = airConditionerRepository;
            _airConditionerHandler = airConditionerHandler;
            _airConditionerDataRepository = airConditionerDataRepository;
            _airConditionerWorkRepository = airConditionerWorkRepository;
        }

        public async Task<AirConditioner> Create(AirConditioner entity)
        {
            entity = await _airConditionerRepository.Create(entity);
            Dictionary<string, object> additionalAttributes = new Dictionary<string, object>
            {
                { "power_per_hour", entity.PowerPerHour},
            };
            bool success = await _airConditionerHandler.ConnectToSmartDevice(entity, additionalAttributes);
            if (success)
            {
                entity.IsConnected = true;
                await _airConditionerRepository.Update(entity);
            }
            return entity;
        }

        public async Task ToggleAmbientSensor(Guid id, bool turnOn = true)
        {
            AirConditioner airConditioner = await _airConditionerRepository.FindWithSmartHome(id);
            if (airConditioner == null)
            {
                throw new ResourceNotFoundException("Smart device not found!");
            }
            await _airConditionerHandler.ToggleSmartDevice(airConditioner, turnOn);
            airConditioner.IsOn = turnOn;
            _ = _airConditionerRepository.Update(airConditioner);
        }

        public async Task<AirConditionerDTO> GetWithData(Guid id)
        {
            AirConditioner airConditioner = await _airConditionerRepository.FindWIthHome(id);
            AirConditionerDTO airConditionerDTO = new AirConditionerDTO
            {
                Id = airConditioner.Id,
                Name = airConditioner.Name,
                IsConnected = airConditioner.IsConnected,
                IsOn = airConditioner.IsOn,
                Category = airConditioner.Category.ToString(),
                Type = airConditioner.Type.ToString(),
                PowerPerHour = airConditioner.PowerPerHour,
                Schedules = airConditioner.ScheduledWorks.Select(work => work.DateTo != null
                                ? new AirConditionerScheduleDTO
                                {
                                    Temperature = work.Temperature,
                                    Mode = work.Mode.ToString(),
                                    Date = $"{work.DateFrom.ToString("dd/MM/yyyy")} {work.Start.ToString("HH:mm")} - {work.DateTo.ToString("dd/MM/yyyy")} {work.End.ToString("HH:mm")}"
                                }
                                : new AirConditionerScheduleDTO
                                {
                                    Temperature = work.Temperature,
                                    Mode = work.Mode.ToString(),
                                    Date = $"{work.DateFrom.ToString("dd/MM/yyyy")} {work.Start.ToString("HH:mm")}"
                                }).ToList() ,
                MaxTemp =airConditioner.MaxTemperature,
                MinTemp=airConditioner.MinTemperature,
            };

            AirConditionerData airConditionerData = GetLastData(id);

            if (airConditionerData != null)
            {
                airConditionerDTO.CurrentTemperature = airConditionerData.Temperature;
                airConditionerDTO.Mode = airConditionerData.Mode;
            }

            return airConditionerDTO;
        }

        #region DataHistory

        public async Task ChangeTemperature(Guid id, double temperature)
        {
            AirConditioner airConditioner= await _airConditionerRepository.FindWithSmartHome(id) ?? throw new ResourceNotFoundException("Smart device not found!");
            _airConditionerHandler.ChangeTemperature(airConditioner, temperature);
            airConditioner.CurrentTemperature = temperature;
            await _airConditionerRepository.Update(airConditioner);
        }

        public async Task ChangeMode(Guid id, string mode)
        {
            AirConditioner airConditioner = await _airConditionerRepository.FindWithSmartHome(id) ?? throw new ResourceNotFoundException("Smart device not found!");
            _airConditionerHandler.ChangeMode(airConditioner, mode);
            switch (mode)
            {
                case "auto":
                    airConditioner.CurrentMode = AirConditionerMode.AUTO; break;
                case "heat":
                    airConditioner.CurrentMode = AirConditionerMode.HEAT; break;
                case "fan":
                    airConditioner.CurrentMode = AirConditionerMode.FAN; break;
                case "cool":
                    airConditioner.CurrentMode = AirConditionerMode.COOL; break;
            }
            await _airConditionerRepository.Update(airConditioner);
        }

        private AirConditionerData GetLastData(Guid id)
        {
            return _airConditionerDataRepository.GetLastData(id);
        }


        public List<AirConditionerData> GetHistoricalData(Guid id, DateTime from, DateTime to)
        {
            return _airConditionerDataRepository.GetHistoricalData(id, from, to);
        }

        public List<AirConditionerData> GetLastHourData(Guid id)
        {
            return _airConditionerDataRepository.GetLastHourData(id);
        }

        public void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _airConditionerDataRepository.AddPoint(fields, tags);
        }

        public async Task AddScheduledWork(String id,double temperature, string mode, string startDate, string endDate)
        {
            AirConditioner airConditioner = await _airConditionerRepository.FindWithSmartHome(Guid.Parse(id)) ?? throw new ResourceNotFoundException("Smart device not found!");
            AirConditionerMode airConditionerMode = AirConditionerMode.AUTO;
            switch (mode)
            {
                case "heat":
                    airConditionerMode=AirConditionerMode.HEAT; break;
                case "fan":
                    airConditionerMode=AirConditionerMode.FAN; break;
                case "cool":
                    airConditionerMode=AirConditionerMode.COOL; break;
            }
            DateTime sDate = DateTime.ParseExact(startDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            if (endDate != null)
            {
                DateTime eDate = DateTime.ParseExact(endDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);

                AirConditionerWork schedule = new AirConditionerWork
                {
                    Mode = airConditionerMode,
                    Temperature = temperature,
                    Name = "AirConditioner-" + mode,
                    Days = new List<DaysInWeek>(),
                    DateFrom = DateOnly.FromDateTime(sDate.Date),
                    DateTo = DateOnly.FromDateTime(eDate.Date),
                    Start = TimeOnly.FromTimeSpan(sDate.TimeOfDay),
                    End = TimeOnly.FromTimeSpan(eDate.TimeOfDay),
                };
                airConditioner.ScheduledWorks.Add(schedule);
                AirConditioner updated = await _airConditionerRepository.Update(airConditioner);
                _airConditionerHandler.AddSchedule(updated, startDate, mode, temperature);
                _airConditionerHandler.AddSchedule(updated, endDate, "turn_off", temperature);
            }
            else
            {
                AirConditionerWork schedule = new AirConditionerWork
                {
                    Mode = airConditionerMode,
                    Temperature = temperature,
                    Name = "AirConditioner-" + mode,
                    Days = new List<DaysInWeek>(),
                    DateFrom = DateOnly.FromDateTime(sDate.Date),
                    Start = TimeOnly.FromTimeSpan(sDate.TimeOfDay),
                };
                airConditioner.ScheduledWorks.Add(schedule);
                AirConditioner updated = await _airConditionerRepository.Update(airConditioner);
                _airConditionerHandler.AddSchedule(updated, startDate, mode, temperature);
            }

        }

        #endregion

        #region CRUD

        public Task<AirConditioner> Delete(Guid id)
        {
            return _airConditionerRepository.Delete(id);
        }

        public Task<AirConditioner> Get(Guid id)
        {
            return _airConditionerRepository.Read(id);
        }

        public Task<IEnumerable<AirConditioner>> GetAll()
        {
            return _airConditionerRepository.ReadAll();
        }

        public IEnumerable<AirConditioner> GetAllWithHome()
        {
            return _airConditionerRepository.FindAllWIthHome();
        }
        public Task<AirConditioner> GetWithHome(Guid id)
        {
            return _airConditionerRepository.FindWIthHome(id);
        }

        public Task<AirConditioner> Update(AirConditioner entity)
        {
            return _airConditionerRepository.Update(entity);
        }
        #endregion
    }
}
