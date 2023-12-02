from DTOs.SmartDeviceDTO import SmartDeviceDTO
from Enums.DeviceType import DeviceType
from Models.PKA.AirConditioner import AirConditioner
from Models.PKA.AmbientSensor import AmbientSensor
from Models.PKA.WashingMachine import WashingMachine
from Models.SPU.Lamp import Lamp
from Models.SPU.Sprinkler import Sprinkler
from Models.SPU.VehicleGate import VehicleGate
from Models.SmartDevice import SmartDevice
from Models.VEU.BatterySystem import BatterySystem
from Models.VEU.SolarPanelSystem import SolarPanelSystem
from Models.VEU.VehicleCharger import VehicleCharger
import asyncio


class SmartDeviceManager:
    def __init__(self):
        self.smart_devices = {}

    def add_device(self, deviceDTO: SmartDeviceDTO):
        device_type_mapping = {
            DeviceType.AirConditioner: AirConditioner,
            DeviceType.AmbientSensor: AmbientSensor,
            DeviceType.WashingMachine: WashingMachine,
            DeviceType.Lamp: Lamp,
            DeviceType.Sprinkler: Sprinkler,
            DeviceType.VehicleGate: VehicleGate,
            DeviceType.BatterySystem: BatterySystem,
            DeviceType.SolarPanelSystem: SolarPanelSystem,
            DeviceType.VehicleCharger: VehicleCharger,
        }

        device_class = device_type_mapping.get(deviceDTO.device_type, SmartDevice)
        device = device_class(deviceDTO.device_id, deviceDTO.smart_home_id, deviceDTO.device_category,
                              deviceDTO.device_type, **deviceDTO.kwargs)

        device.connect(deviceDTO.host, deviceDTO.port, deviceDTO.keepalive)
        self.smart_devices[device.device_id] = device

    def remove_device(self, device_id):
        smart_device = self.smart_devices.pop(device_id, None)
        if smart_device:
            smart_device.disconnect()

    def turn_on_device(self, device_id):
        smart_device = self.smart_devices.get(device_id, None)
        if smart_device:
            smart_device.turn_on()

    def turn_off_device(self, device_id):
        smart_device = self.smart_devices.get(device_id, None)
        if smart_device:
            smart_device.turn_off()
