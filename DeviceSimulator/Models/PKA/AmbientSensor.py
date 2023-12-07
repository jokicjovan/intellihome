import asyncio

from Models.SmartDevice import SmartDevice


class AmbientSensor(SmartDevice):
    def __init__(self, device_id, smart_home_id, device_category, device_type):
        super().__init__(device_id, smart_home_id, device_category, device_type)

    async def send_data(self):
        i = 10
        while True:
            if not self.is_on:
                break
            self.client.publish(self.from_device_topic, str({"temperature": i, "humidity": 60}), retain=False)
            i += 1
            await asyncio.sleep(10)
