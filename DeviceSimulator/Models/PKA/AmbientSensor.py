import asyncio

from Models.SmartDevice import SmartDevice


class AmbientSensor(SmartDevice):
    def __init__(self, device_id, smart_home_id, device_category, device_type):
        super().__init__(device_id, smart_home_id, device_category, device_type)

    async def send_data(self):
        while True:
            if not self.is_on:
                break
            topic = (f"FromDevice/{self.smart_home_id}/{self.device_category.value}/{self.device_type.value}/"
                     f"{self.device_id}")
            self.client.publish(topic, str({"temperature": 10, "humidity": 60}), retain=False)
            await asyncio.sleep(60)
