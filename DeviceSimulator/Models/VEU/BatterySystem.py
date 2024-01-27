import asyncio
import json

from Models.SmartDevice import SmartDevice


class BatterySystem(SmartDevice):
    def __init__(self, device_id, smart_home_id, device_category, device_type, capacity):
        super().__init__(device_id, smart_home_id, device_category, device_type)
        self.capacity = capacity
        self.current_capacity = 0
        self.lock = asyncio.Lock()

    async def send_data(self):
        if not self.is_on.is_set():
            return
        self.client.publish(self.send_topic, json.dumps({"currentCapacity": round(self.current_capacity, 4)}),
                            retain=False)
