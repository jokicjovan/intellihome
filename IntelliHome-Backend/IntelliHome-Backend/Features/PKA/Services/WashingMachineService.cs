using Data.Models.PKA;
using Data.Models.Shared;
using IntelliHome_Backend.Features.Home.DataRepository.Interfaces;
using IntelliHome_Backend.Features.PKA.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.PKA.DTOs;
using IntelliHome_Backend.Features.PKA.Handlers.Interfaces;
using IntelliHome_Backend.Features.PKA.Repositories.Interfaces;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;
using IntelliHome_Backend.Features.Shared.DTOs;
using IntelliHome_Backend.Features.Shared.Exceptions;

namespace IntelliHome_Backend.Features.PKA.Services
{
    public class WashingMachineService : IWashingMachineService
    {
        private readonly IWashingMachineRepository _washingMachineRepository;
        private readonly IWashingMachineDataRepository _washingMachineDataRepository;
        private readonly IWashingMachineModeRepository _washingMachineModeRepository;
        private readonly IWashingMachineHandler _washingMachineHandler;
        private readonly ISmartDeviceDataRepository _smartDeviceDataRepository;

        public WashingMachineService(IWashingMachineRepository washingMachineRepository, 
            IWashingMachineModeRepository washingMachineModeRepository, IWashingMachineHandler washingMachineHandler, 
            IWashingMachineDataRepository washingMachineDataRepository,
            ISmartDeviceDataRepository smartDeviceDataRepository)
        {
            _washingMachineRepository = washingMachineRepository;
            _washingMachineModeRepository = washingMachineModeRepository;
            _washingMachineHandler = washingMachineHandler;
            _washingMachineDataRepository = washingMachineDataRepository;
            _smartDeviceDataRepository = smartDeviceDataRepository;
        }

        public List<WashingMachineMode> GetWashingMachineModes(List<Guid> modesIds)
        {
            return _washingMachineModeRepository.FindWashingMachineModes(modesIds);
        }

        public Task<IEnumerable<WashingMachineMode>> GetAllWashingMachineModes()
        {
            return _washingMachineModeRepository.ReadAll();
        }

        public async Task<WashingMachine> Create(WashingMachine entity)
        {
            entity = await _washingMachineRepository.Create(entity);
            bool success = await _washingMachineHandler.ConnectToSmartDevice(entity);
            if (!success) return entity;
            entity.IsConnected = true;
            await _washingMachineRepository.Update(entity);
            var fields = new Dictionary<string, object>
            {
                { "isConnected", 1 }

            };
            var tags = new Dictionary<string, string>
            {
                { "deviceId", entity.Id.ToString()}
            };
            _smartDeviceDataRepository.AddPoint(fields, tags);
            return entity;
        }

        public async Task ToggleWashingMachine(Guid id, string username, bool turnOn = true)
        {
            WashingMachine washingMachine = await _washingMachineRepository.FindWithSmartHome(id) ?? throw new ResourceNotFoundException("Smart device not found!");
            _ = _washingMachineHandler.ToggleSmartDevice(washingMachine, turnOn);
            washingMachine.IsOn = turnOn;
            _ = _washingMachineRepository.Update(washingMachine);

            var fields = new Dictionary<string, object>
            {
                { "action", turnOn ? "ON" : "OFF" }

            };
            var tags = new Dictionary<string, string>
            {
                { "actionBy", username},
                { "deviceId", id.ToString()}
            };
            _washingMachineDataRepository.AddActionMeasurement(fields, tags);
        }

        public async Task<WashingMachineDTO> GetWithData(Guid id)
        {
            WashingMachine washingMachine = await _washingMachineRepository.FindWIthHome(id);
            WashingMachineDTO washingMachineDTO = new WashingMachineDTO
            {
                Id = washingMachine.Id,
                Name = washingMachine.Name,
                IsConnected = washingMachine.IsConnected,
                IsOn = washingMachine.IsOn,
                Category = washingMachine.Category.ToString(),
                Type = washingMachine.Type.ToString(),
                PowerPerHour = washingMachine.PowerPerHour,
                Modes = washingMachine.Modes.Select(mode => mode.Name.ToString().ToLower()).ToList(),
                Schedules = washingMachine.ScheduledWorks.Select(work => work.DateTo.Year != 1
                                ? new WashingMachineScheduleDTO
                                {
                                    Temperature = work.Temperature,
                                    Mode = work.Mode.Name.ToString(),
                                    Date = $"{work.DateFrom.ToString("dd/MM/yyyy")} {work.Start.ToString("HH:mm")} - {work.DateTo.ToString("dd/MM/yyyy")} {work.End.ToString("HH:mm")}"
                                }
                                : new WashingMachineScheduleDTO
                                {
                                    Temperature = work.Temperature,
                                    Mode = work.Mode.Name.ToString(),
                                    Date = $"{work.DateFrom.ToString("dd/MM/yyyy")} {work.Start.ToString("HH:mm")}"
                                }).ToList(),
                SmartHomeId = washingMachine.SmartHome.Id,
            };

            WashingMachineData washingMachineData = GetLastData(id);

            if (washingMachineData != null)
            {
                washingMachineDTO.CurrentTemperature = washingMachineData.Temperature;
                washingMachineDTO.Mode = washingMachineData.Mode;
            }

            return washingMachineDTO;
        }

        public async Task ChangeMode(Guid id, string mode, string username)
        {
            WashingMachine washingMachine = await _washingMachineRepository.FindWithSmartHome(id) ?? throw new ResourceNotFoundException("Smart device not found!");
            WashingMachineMode washingMachineMode = await _washingMachineModeRepository.FindWashingMachineModeByName(mode);
            _washingMachineHandler.ChangeMode(washingMachine, mode,washingMachineMode.Temperature);
            washingMachine.Mode =await _washingMachineModeRepository.FindWashingMachineModeByName(mode);
            var fields = new Dictionary<string, object>
            {
                { "action", $"CHANGE MODE {washingMachine.Mode.Name.ToString()}" }

            };
            var tags = new Dictionary<string, string>
            {
                { "actionBy", username},
                { "deviceId", id.ToString()}
            };
            _washingMachineDataRepository.AddActionMeasurement(fields, tags);
            await _washingMachineRepository.Update(washingMachine);
        }

        public List<ActionDataDTO> GetActionHistoricalData(Guid id, DateTime from, DateTime to)
        {
            return _washingMachineDataRepository.GetActionHistoricalData(id, from, to);
        }

        private WashingMachineData GetLastData(Guid id)
        {
            return _washingMachineDataRepository.GetLastData(id);
        }


        public List<WashingMachineData> GetHistoricalData(Guid id, DateTime from, DateTime to)
        {
            return _washingMachineDataRepository.GetHistoricalData(id, from, to);
        }

        public List<WashingMachineData> GetLastHourData(Guid id)
        {
            return _washingMachineDataRepository.GetLastHourData(id);
        }

        public void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _washingMachineDataRepository.AddPoint(fields, tags);
        }

        public async Task AddScheduledWork(String id, double temperature, string mode, string startDate, string endDate, string username)
        {
            WashingMachine washingMachine = await _washingMachineRepository.FindWithSmartHome(Guid.Parse(id)) ?? throw new ResourceNotFoundException("Smart device not found!");
            WashingMachineMode washingMachineMode =await _washingMachineModeRepository.FindWashingMachineModeByName(mode);
            DateTime sDate = DateTime.ParseExact(startDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            DateTime eDate = DateTime.ParseExact(startDate, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            eDate = eDate.AddMinutes(5);
            

            WashingMachineWork schedule = new WashingMachineWork
            {
                Mode = washingMachineMode,
                Temperature = washingMachineMode.Temperature,
                Name = "WashingMachine-" + mode,
                Days = new List<DaysInWeek>(),
                DateFrom = DateOnly.FromDateTime(sDate.Date),
                DateTo = DateOnly.FromDateTime(eDate.Date),
                Start = TimeOnly.FromTimeSpan(sDate.TimeOfDay),
                End = TimeOnly.FromTimeSpan(eDate.TimeOfDay),
            };
            washingMachine.ScheduledWorks.Add(schedule);
            WashingMachine updated = await _washingMachineRepository.Update(washingMachine);
            _washingMachineHandler.AddSchedule(updated, startDate, mode, temperature);
            _washingMachineHandler.AddSchedule(updated, eDate.ToString("dd/MM/yyyy HH:mm"), "turn_off", temperature);
            var fields = new Dictionary<string, object>
            {
                { "action", $"SCHEDULE MODE:{mode.ToUpper()}" }

            };
            var tags = new Dictionary<string, string>
            {
                { "actionBy", username},
                { "deviceId", id.ToString()}
            };
            _washingMachineDataRepository.AddActionMeasurement(fields, tags);

        }

        public Task<WashingMachine> Get(Guid id)
        {
            return _washingMachineRepository.Read(id);
        }

        public Task<IEnumerable<WashingMachine>> GetAll()
        {
            return _washingMachineRepository.ReadAll();
        }

        public IEnumerable<WashingMachine> GetAllWithHome()
        {
            return _washingMachineRepository.FindAllWIthHome();
        }
        public Task<WashingMachine> GetWithHome(Guid id)
        {
            return _washingMachineRepository.FindWIthHome(id);
        }

        public Task<WashingMachine> Update(WashingMachine entity)
        {
            throw new NotImplementedException();
        }

        public Task<WashingMachine> Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
