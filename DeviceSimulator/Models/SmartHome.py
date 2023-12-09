import asyncio
import json

import paho.mqtt.client as mqtt
from DTOs.SmartDeviceDTO import SmartDeviceDTO
from Enums.DeviceCategory import DeviceCategory
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


class SmartHome:
    def __init__(self, smart_home_id):
        self.smart_devices = {}
        self.battery_systems = []
        self.device_topic = f"FromDevice/{smart_home_id}/+/+/+"
        self.client = mqtt.Client(client_id=smart_home_id, clean_session=True)
        self.event_loop = asyncio.get_event_loop()

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
        smart_device = device_class(device_dto.device_id, device_dto.smart_home_id, device_dto.device_category,
                                    device_dto.device_type, **device_dto.kwargs)

        smart_device.setup_connection(device_dto.host, device_dto.port, device_dto.keepalive)
        smart_device.turn_on()
        self.smart_devices[smart_device.device_id] = smart_device
        if device_dto.device_type == DeviceType.BatterySystem:
            self.battery_systems.append(smart_device)
        return smart_device

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

    def setup_connection(self, host, port, keepalive):
        self.client.on_message = self.on_device_data_receive
        self.client.on_connect = self.on_home_connect
        self.client.connect(host, port, keepalive=keepalive)
        self.client.loop_start()

    def on_home_connect(self, client, userdata, flags, rc):
        self.client.subscribe(topic=self.device_topic)

    def on_device_data_receive(self, client, user_data, msg):
        asyncio.run_coroutine_threadsafe(self.handle_message_from_device(msg), loop=self.event_loop)

    async def handle_message_from_device(self, msg):
        topic_parts = msg.topic.split("/")
        data = json.loads(msg.payload.decode())
        if self.battery_systems:
            if topic_parts[2] == DeviceCategory.VEU.value and topic_parts[3] == DeviceType.SolarPanelSystem:
                total_production = data.get("production_per_minute")
                production_by_battery = total_production / len(self.battery_systems)
                for battery_system in self.battery_systems:
                    async with battery_system.lock:
                        battery_system.current_production += production_by_battery

            elif topic_parts[3] != DeviceType.BatterySystem:
                total_consumption = data.get("consumption_per_minute")
                consumption_by_battery = total_consumption / len(self.battery_systems)
                for battery_system in self.battery_systems:
                    async with battery_system.lock:
                        battery_system.current_consumption += consumption_by_battery
