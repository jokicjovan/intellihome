import asyncio
import json

from Models.SmartDevice import SmartDevice


class AmbientSensor(SmartDevice):
    def __init__(self, device_id, smart_home_id, device_category, device_type, power_per_hour):
        super().__init__(device_id, smart_home_id, device_category, device_type)
        self.power_per_hour = power_per_hour

    async def send_data(self):
        i = 10
        while True:
            if not self.is_on:
                break
            self.client.publish(self.send_topic, json.dumps({"temperature": i, "humidity": 60,
                                                             "consumption_per_minute": round(self.power_per_hour / 60,
                                                                                             4)}), retain=False)
            i += 1
            await asyncio.sleep(10)
