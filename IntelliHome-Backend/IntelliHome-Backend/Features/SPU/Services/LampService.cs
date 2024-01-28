using Data.Models.Shared;
using Data.Models.SPU;
using IntelliHome_Backend.Features.Home.DataRepository.Interfaces;
using IntelliHome_Backend.Features.Shared.Exceptions;
using IntelliHome_Backend.Features.SPU.DataRepositories.Interfaces;
using IntelliHome_Backend.Features.SPU.DTOs;
using IntelliHome_Backend.Features.SPU.Handlers.Interfaces;
using IntelliHome_Backend.Features.SPU.Repositories.Interfaces;
using IntelliHome_Backend.Features.SPU.Services.Interfaces;

namespace IntelliHome_Backend.Features.SPU.Services
{
    public class LampService : ILampService
    {
        private readonly ILampRepository _lampRepository;
        private readonly ILampDataRepository _lampDataRepository;
        private readonly ILampHandler _lampHandler;
        private readonly ISmartDeviceDataRepository _smartDeviceDataRepository;

        public LampService(
            ILampRepository lampRepository, 
            ILampHandler lampHandler, 
            ILampDataRepository lampDataRepository, 
            ISmartDeviceDataRepository smartDeviceDataRepository)
        {
            _lampRepository = lampRepository;
            _lampHandler = lampHandler;
            _lampDataRepository = lampDataRepository;
            _smartDeviceDataRepository = smartDeviceDataRepository;
        }

        public async Task<Lamp> Create(Lamp entity)
        {
            entity = await _lampRepository.Create(entity);
            bool success = await _lampHandler.ConnectToSmartDevice(entity);
            if (!success) return entity;
            entity.IsConnected = true;
            await _lampRepository.Update(entity);
            var fields = new Dictionary<string, object>
            {
                { "isConnected", true }

            };
            var tags = new Dictionary<string, string>
            {
                { "deviceId", entity.Id.ToString()}
            };
            _smartDeviceDataRepository.AddPoint(fields, tags);
            return entity;
        }


        public async Task<LampDTO> GetWithData(Guid id)
        {
            Lamp lamp = await _lampRepository.FindWithSmartHome(id) ?? throw new ResourceNotFoundException("Smart device not found!");
            LampDTO lampDTO = new LampDTO
            {
                Id = lamp.Id,
                Name = lamp.Name,
                IsConnected = lamp.IsConnected,
                IsOn = lamp.IsOn,
                Category = lamp.Category.ToString(),
                Type = lamp.Type.ToString(),
                SmartHomeId = lamp.SmartHome.Id,
                PowerPerHour = lamp.PowerPerHour,
                BrightnessLimit = lamp.BrightnessLimit,
            };

            LampData lampData = null;
            try
            {
                lampData = GetLastData(id);
            }
            catch (Exception)
            {
                lampData = null;
            }


            if (lampData != null)
            {
                lampDTO.CurrentBrightness = lampData.CurrentBrightness;
                lampDTO.IsShining = lampData.IsShining;
                lampDTO.IsAuto = lampData.IsAuto;
            }

            return lampDTO;
        }

        private LampData GetLastData(Guid id)
        {
            return _lampDataRepository.GetLastData(id);
        }

        public List<LampData> GetHistoricalData(Guid id, DateTime from, DateTime to)
        {
            return _lampDataRepository.GetHistoricalData(id, from, to);
        }

        public void AddPoint(Dictionary<string, object> fields, Dictionary<string, string> tags)
        {
            _lampDataRepository.AddPoint(fields, tags);
        }

        public async Task ChangeMode(Guid id, bool isAuto)
        { 
            Lamp lamp = await _lampRepository.FindWithSmartHome(id) ?? throw new ResourceNotFoundException("Smart device not found!");
            _lampHandler.ChangeMode(lamp, isAuto);
            lamp.IsAuto = isAuto;
            await _lampRepository.Update(lamp);
        }

        public async Task ChangeBrightnessLimit(Guid id, double brightness)
        {
            Lamp lamp = await _lampRepository.FindWithSmartHome(id) ?? throw new ResourceNotFoundException("Smart device not found!");
            _lampHandler.ChangeBrightnessLimit(lamp, brightness);
            lamp.BrightnessLimit = brightness;
            await _lampRepository.Update(lamp);
        }

        public async Task ToggleLamp(Guid id, bool turnOn)
        {
            Lamp lamp = await _lampRepository.FindWithSmartHome(id) ?? throw new ResourceNotFoundException("Smart device not found!");
            await _lampHandler.ToggleSmartDevice(lamp, turnOn);

            lamp.IsOn = turnOn;
            await _lampRepository.Update(lamp);
        }

        public async Task TurnLightOnOff(Guid id, bool turnOn)
        {
            Lamp lamp = await _lampRepository.FindWithSmartHome(id) ?? throw new ResourceNotFoundException("Smart device not found!");
            _lampHandler.TurnLightOnOff(lamp, turnOn);
        }


        public async Task<Lamp> Delete(Guid id)
        {
            return await _lampRepository.Delete(id);
        }

        public async Task<Lamp> Get(Guid id)
        {
            return await _lampRepository.Read(id);
        }

        public async Task<IEnumerable<Lamp>> GetAll()
        {
            return await _lampRepository.ReadAll();
        }

        public async Task<Lamp> Update(Lamp entity)
        {
            return await _lampRepository.Update(entity);
        }
    }
}
