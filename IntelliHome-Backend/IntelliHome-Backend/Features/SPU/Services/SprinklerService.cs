using Data.Models.Shared;
using Data.Models.SPU;
using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.Shared.Exceptions;
using IntelliHome_Backend.Features.SPU.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.SPU.DTOs;
using IntelliHome_Backend.Features.SPU.Handlers.Interfaces;
using IntelliHome_Backend.Features.SPU.Repositories.Interfaces;
using IntelliHome_Backend.Features.SPU.Services.Interfaces;

namespace IntelliHome_Backend.Features.SPU.Services
{
    public class SprinklerService : ISprinklerService
    {
        private readonly ISprinklerRepository _sprinklerRepository;
        private readonly ISprinklerHandler _sprinklerHandler;
        private readonly ISprinklerDataRepository _sprinklerDataRepository;
        private readonly ISprinklerWorkRepository _sprinklerWorkRepository;

        public SprinklerService(ISprinklerRepository sprinklerRepository, ISprinklerHandler sprinklerHandler, ISprinklerWorkRepository sprinklerWorkRepository, ISprinklerDataRepository sprinklerDataRepository)
        {
            _sprinklerRepository = sprinklerRepository;
            _sprinklerHandler = sprinklerHandler;
            _sprinklerWorkRepository = sprinklerWorkRepository;
            _sprinklerDataRepository = sprinklerDataRepository;
        }

        public async Task<Sprinkler> Create(Sprinkler entity)
        {
            entity = await _sprinklerRepository.Create(entity);
            bool success = await _sprinklerHandler.ConnectToSmartDevice(entity);
            if (!success) return entity;
            entity.IsConnected = true;
            await _sprinklerRepository.Update(entity);
            return entity;
        }

        public async Task<Sprinkler> GetWithSmartHome(Guid id)
        {
            return await _sprinklerRepository.ReadWithSmartHome(id);
        }

        public List<SprinklerData> GetHistoricalData(Guid id, DateTime from, DateTime to)
        {
            return _sprinklerDataRepository.GetHistoricalData(id, from, to);
        }

        public async Task AddScheduledWork(string scheduleId, bool scheduleIsSpraying, string scheduleStartDate, string? scheduleEndDate,
            string username)
        {
            Sprinkler sprinkler = await _sprinklerRepository.ReadWithSmartHome(Guid.Parse(scheduleId)) ?? throw new ResourceNotFoundException("Sprinkler not found!");
            DateTime sDate = DateTime.ParseExact(scheduleStartDate, "dd/MM/yyyy HH:mm",
                System.Globalization.CultureInfo.InvariantCulture);
            if (scheduleEndDate != null)
            {
                DateTime eDate = DateTime.ParseExact(scheduleEndDate, "dd/MM/yyyy HH:mm",
                                       System.Globalization.CultureInfo.InvariantCulture);
                if (eDate < sDate)
                {
                    throw new InvalidInputException("End date must be after start date!");
                }

                SprinklerWork sprinklerWork = new SprinklerWork
                {
                    Name = "Sprinkler work",
                    IsSpraying = scheduleIsSpraying,
                    DateFrom = DateOnly.FromDateTime(sDate.Date),
                    DateTo = DateOnly.FromDateTime(eDate.Date),
                    Start = TimeOnly.FromTimeSpan(sDate.TimeOfDay),
                    End = TimeOnly.FromTimeSpan(eDate.TimeOfDay),
                    Days = new List<DaysInWeek>(),
                };
                sprinkler.ScheduledWorks.Add(sprinklerWork);
                _ = await _sprinklerRepository.Update(sprinkler);
                _sprinklerHandler.AddSchedule(sprinkler, scheduleStartDate, true);
                _sprinklerHandler.AddSchedule(sprinkler, scheduleEndDate, false);
            }
            else
            {
                SprinklerWork sprinklerWork = new SprinklerWork
                {
                    IsSpraying = scheduleIsSpraying,
                    DateFrom = DateOnly.FromDateTime(sDate.Date),
                    Start = TimeOnly.FromTimeSpan(sDate.TimeOfDay),
                    Name = "Sprinkler work",
                    Days = new List<DaysInWeek>(),
                };
                sprinkler.ScheduledWorks.Add(sprinklerWork);
                _ = await _sprinklerRepository.Update(sprinkler);
                _sprinklerHandler.AddSchedule(sprinkler, scheduleStartDate, true);
            }
        }

        public async Task ToggleSprinkler(Guid id, string username, bool turnSprayingOn)
        {
            Sprinkler sprinkler = await _sprinklerRepository.ReadWithSmartHome(id) ?? throw new ResourceNotFoundException("Sprinkler not found!");
            _sprinklerHandler.ToggleSmartDevice(sprinkler, turnSprayingOn);

            sprinkler.IsOn = turnSprayingOn;
            await _sprinklerRepository.Update(sprinkler);
        }

        public async Task<SprinklerDTO> GetWithData(Guid id)
        {
            Sprinkler sprinkler = await _sprinklerRepository.ReadWithSmartHome(id) ?? throw new ResourceNotFoundException("Smart device not found!");
            SprinklerDTO sprinklerDTO = new SprinklerDTO
            {
                Id = sprinkler.Id,
                Name = sprinkler.Name,
                IsConnected = sprinkler.IsConnected,
                Category = sprinkler.Category.ToString(),
                Type = sprinkler.Type.ToString(),
                SmartHomeId = sprinkler.SmartHome.Id,
                PowerPerHour = sprinkler.PowerPerHour,
                IsOn = sprinkler.IsOn,
                Schedules = sprinkler.ScheduledWorks.Select(work => work.DateTo.Year != 1
                    ? new SprinklerScheduleDTO
                    {
                        Date = $"{work.DateFrom:dd/MM/yyyy} {work.Start:HH:mm} - {work.DateTo:dd/MM/yyyy} {work.End:HH:mm}",
                        IsSpraying = work.IsSpraying
                    }
                    : new SprinklerScheduleDTO
                    {
                        Date = $"{work.DateFrom:dd/MM/yyyy} {work.Start:HH:mm}",
                        IsSpraying = work.IsSpraying
                    }).ToList()
            };

            SprinklerData sprinklerData = null;
            try
            {
                sprinklerData = GetLastData(id);
            }
            catch (Exception)
            {
                sprinklerData = null;
            }

            if (sprinklerData != null)
            {
                sprinklerDTO.IsSpraying = sprinklerData.IsSpraying;
            }

            return sprinklerDTO;
        }


        public void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _sprinklerDataRepository.AddPoint(fields, tags);
        }

        public async Task ToggleSprinklerSpraying(Guid id, string username, bool turnOn)
        {
            Sprinkler sprinkler = await _sprinklerRepository.ReadWithSmartHome(id) ?? throw new ResourceNotFoundException("Sprinkler not found!");
            _sprinklerHandler.SetSpraying(sprinkler, turnOn);
        }

        private SprinklerData GetLastData(Guid id)
        {
            return _sprinklerDataRepository.GetLastData(id);
        }

        #region CRUD




        public Task<Sprinkler> Delete(Guid id)
        {
            return _sprinklerRepository.Delete(id);
        }

        public Task<Sprinkler> Get(Guid id)
        {
            return _sprinklerRepository.Read(id);
        }

        public Task<IEnumerable<Sprinkler>> GetAll()
        {
            return _sprinklerRepository.ReadAll();
        }

        public Task<Sprinkler> Update(Sprinkler entity)
        {
            return _sprinklerRepository.Update(entity);
        }
        #endregion
    }
}
