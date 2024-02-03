from DTOs.SmartDeviceDTO import SmartDeviceDTO
from DTOs.SmartDeviceShortDTO import SmartDeviceShortDTO
from Models.SmartHome import SmartHome


class SmartHomesManager:
    def __init__(self):
        self.smart_homes = {}

    def add_device_to_home(self, device_dto: SmartDeviceDTO):
        smart_home = self.smart_homes.get(device_dto.smart_home_id)
        if smart_home is None:
            smart_home = SmartHome(device_dto.smart_home_id)
            smart_home.setup_connection(device_dto.host, device_dto.port, device_dto.keepalive)
        smart_home.add_device(device_dto)
        self.smart_homes[device_dto.smart_home_id] = smart_home

    def remove_device_from_home(self, device_short_dto: SmartDeviceShortDTO):
        smart_home = self.smart_homes.get(device_short_dto.smart_home_id)
        if smart_home:
            smart_home.remove_device(device_short_dto.device_id)
            if not smart_home.smart_devices:
                del self.smart_homes[device_short_dto.smart_home_id]

    def turn_on_device_in_home(self, device_short_dto: SmartDeviceShortDTO):
        smart_home = self.smart_homes.get(device_short_dto.smart_home_id)
        if smart_home:
            smart_home.turn_on_device(device_short_dto.device_id)

    def turn_off_device_in_home(self, device_short_dto: SmartDeviceShortDTO):
        smart_home = self.smart_homes.get(device_short_dto.smart_home_id)
        if smart_home:
            smart_home.turn_off_device(device_short_dto.device_id)
