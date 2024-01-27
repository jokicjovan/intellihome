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
        self.smart_home_id = smart_home_id
        self.smart_devices = {}
        self.battery_systems = []
        self.device_topic = f"FromDevice/{smart_home_id}/+/+/+"
        self.home_usage_topic = f"FromSmartHome/{smart_home_id}/Usage"
        self.client = mqtt.Client(client_id=smart_home_id, clean_session=True)
        self.event_loop = asyncio.get_event_loop()
        self.current_production = 0
        self.current_consumption = 0

    def add_device(self, device_dto: SmartDeviceDTO):
        if self.smart_devices.get(device_dto.device_id, None) is not None:
            return

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
        smart_device = device_class(device_dto.device_id, self, device_dto.device_category,
                                    device_dto.device_type, **device_dto.kwargs)

        smart_device.setup_connection(device_dto.host, device_dto.port, device_dto.keepalive)
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
            self.event_loop.create_task(smart_device.turn_on())

    def turn_off_device(self, device_id):
        smart_device = self.smart_devices.get(device_id, None)
        if smart_device:
            self.event_loop.create_task(smart_device.turn_off())

    def setup_connection(self, host, port, keepalive):
        self.client.on_message = self.on_device_usage_receive
        self.client.on_connect = self.on_home_connect
        self.client.connect(host, port, keepalive=keepalive)
        self.client.loop_start()

    def on_home_connect(self, client, userdata, flags, rc):
        self.client.subscribe(topic=self.device_topic)
        self.event_loop.create_task(self.send_house_usage())

    def on_device_usage_receive(self, client, user_data, msg):
        self.event_loop.create_task(self.handle_message_from_device(msg))

    async def handle_message_from_device(self, msg):
        topic_parts = msg.topic.split("/")
        data = json.loads(msg.payload.decode())
        if topic_parts[2] == DeviceCategory.VEU.value and topic_parts[3] == DeviceType.SolarPanelSystem:
            self.current_production += data.get("productionPerMinute", 0)
        elif topic_parts[3] != DeviceType.BatterySystem:
            self.current_consumption += data.get("consumptionPerMinute", 0)

    async def send_house_usage(self):
        while True:
            grid_per_minute = 0
            turned_on_batteries = [battery_system for battery_system in self.battery_systems if battery_system.is_on]
            if len(turned_on_batteries) > 0:
                consumption_per_battery = self.current_consumption / len(turned_on_batteries)
                production_per_battery = self.current_production / len(turned_on_batteries)
                for battery_system in turned_on_batteries:
                    async with battery_system.lock:
                        difference = production_per_battery - consumption_per_battery
                        expected_charge = battery_system.current_capacity + difference
                        if expected_charge > battery_system.capacity:
                            grid_per_minute += -(expected_charge - battery_system.capacity)
                            battery_system.current_capacity = battery_system.capacity
                        elif expected_charge < 0:
                            grid_per_minute += -expected_charge
                            battery_system.current_capacity = 0
                        else:
                            battery_system.current_capacity += difference
                        asyncio.create_task(battery_system.send_data())
            else:
                grid_per_minute = -(self.current_production - self.current_consumption)

            # grid pozitivan, preuzeto iz elektrodistribucije
            # grud negativan, vraceno elektrodistribuciji
            self.client.publish(self.home_usage_topic,
                                json.dumps({"productionPerMinute": round(self.current_production, 4),
                                            "consumptionPerMinute": round(self.current_consumption, 4),
                                            "gridPerMinute": round(grid_per_minute, 4)}), retain=False)
            self.current_production = 0
            self.current_consumption = 0
            await asyncio.sleep(10)
