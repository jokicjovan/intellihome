﻿using Data.Models.PKA;
using Data.Models.Shared;
using IntelliHome_Backend.Features.PKA.Handlers.Interfaces;
using IntelliHome_Backend.Features.PKA.Repositories.Interfaces;
using IntelliHome_Backend.Features.PKA.Services.Interfaces;

namespace IntelliHome_Backend.Features.PKA.Services
{
    public class AirConditionerService : IAirConditionerService
    {
        private readonly IAirConditionerRepository _airConditionerRepository;
        private readonly IAirConditionerHandler _airConditionerHandler;

        public AirConditionerService(IAirConditionerRepository airConditionerRepository, IAirConditionerHandler airConditionerHandler)
        {
            _airConditionerRepository = airConditionerRepository;
            _airConditionerHandler = airConditionerHandler;
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

        public Task<AirConditioner> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<AirConditioner> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AirConditioner>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<AirConditioner> Update(AirConditioner entity)
        {
            throw new NotImplementedException();
        }
    }
}
