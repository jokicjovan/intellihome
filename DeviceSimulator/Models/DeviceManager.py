from DTOs.SmartDeviceDTO import SmartDeviceDTO
from Models.SmartDevice import SmartDevice


class DeviceManager:
    def __init__(self):
        self.devices = {}

    def add_device(self, deviceDTO: SmartDeviceDTO):
        device = SmartDevice(deviceDTO.device_id, deviceDTO.smart_home_id)
        device.connect(deviceDTO.host, deviceDTO.port, deviceDTO.keepalive)
        self.devices[device.device_id] = device

    def remove_device(self, device_id):
        device = self.devices.pop(device_id, None)
        if device:
            device.disconnect()
