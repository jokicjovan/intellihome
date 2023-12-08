from DTOs.SmartDeviceDTO import SmartDeviceDTO
from DTOs.SmartDeviceShortDTO import SmartDeviceShortDTO
from Models.SmartDeviceManager import SmartDeviceManager


class SmartHomeManager:
    def __init__(self):
        self.smart_homes = {}

    def add_device_to_home(self, device_dto: SmartDeviceDTO):
        smart_device_manager = self.smart_homes.get(device_dto.smart_home_id, SmartDeviceManager())
        smart_device_manager.add_device(device_dto)
        self.smart_homes[device_dto.smart_home_id] = smart_device_manager

    def remove_device_from_home(self, device_short_dto: SmartDeviceShortDTO):
        smart_device_manager = self.smart_homes.get(device_short_dto.smart_home_id)
        if smart_device_manager:
            smart_device_manager.remove_device(device_short_dto.device_id)
            if not smart_device_manager.smart_devices:
                del self.smart_homes[device_short_dto.smart_home_id]

    def turn_on_device_in_home(self, device_short_dto: SmartDeviceShortDTO):
        smart_device_manager = self.smart_homes.get(device_short_dto.smart_home_id)
        if smart_device_manager:
            smart_device_manager.turn_on_device(device_short_dto.device_id)

    def turn_off_device_in_home(self, device_short_dto: SmartDeviceShortDTO):
        smart_device_manager = self.smart_homes.get(device_short_dto.smart_home_id)
        if smart_device_manager:
            smart_device_manager.turn_off_device(device_short_dto.device_id)
