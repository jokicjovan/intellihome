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


class SmartDeviceManager:
    def __init__(self):
        self.smart_devices = {}
        self.battery_systems = []

    def add_device(self, device_dto: SmartDeviceDTO):
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

        device_class = device_type_mapping.get(device_dto.device_type, SmartDevice)
        device = device_class(device_dto.device_id, device_dto.smart_home_id, device_dto.device_category,
                              device_dto.device_type, **device_dto.kwargs)

        device.setup_connection(device_dto.host, device_dto.port, device_dto.keepalive)
        self.smart_devices[device.device_id] = device
        if device_dto.device_type == DeviceType.BatterySystem:
            self.battery_systems.append(device)
        return device

    def remove_device(self, device_id):
        smart_device = self.smart_devices.pop(device_id, None)
        if smart_device:
            smart_device.disconnect()
            self.battery_systems = [battery_system for battery_system in self.battery_systems
                                    if battery_system.device_id != device_id]

    def turn_on_device(self, device_id):
        smart_device = self.smart_devices.get(device_id, None)
        if smart_device:
            smart_device.turn_on()

    def turn_off_device(self, device_id):
        smart_device = self.smart_devices.get(device_id, None)
        if smart_device:
            smart_device.turn_off()

