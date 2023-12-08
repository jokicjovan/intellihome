from Enums.DeviceCategory import DeviceCategory
from Enums.DeviceType import DeviceType
from Models.SmartDevice import SmartDevice


class BatterySystem(SmartDevice):
    def __init__(self, device_id, smart_home_id, device_category, device_type, capacity):
        super().__init__(device_id, smart_home_id, device_category, device_type)
        self.capacity = capacity
        self.solar_panels_topic = (f"FromDevice/{self.smart_home_id}/{DeviceCategory.VEU.value}/"
                                   f"{DeviceType.SolarPanelSystem.value}/+")

    def setup_connection(self, host, port, keepalive):
        super().setup_connection(host, port, keepalive)
        self.client.subscribe(topic=self.solar_panels_topic)

    def on_data_receive(self, client, userdata, msg):
        print(msg.payload.decode())
