import asyncio
import json
import random

from Models.SmartDevice import SmartDevice


class AmbientSensor(SmartDevice):
    def __init__(self, device_id, smart_home_id, device_category, device_type, power_per_hour):
        super().__init__(device_id, smart_home_id, device_category, device_type)
        self.power_per_hour = power_per_hour

    def generate_sensor_data(self, temp, hum):
        temperature_change = round(random.uniform(-0.5, 0.5), 2)
        humidity_change = round(random.uniform(-1, 1), 2)

        temperature = round(max(18, min(28, temp + temperature_change)), 1)
        humidity = round(max(40, min(60, hum + humidity_change)))

        return temperature, humidity

    async def send_data(self):
        current_temperature = 22
        current_humidity = 50
        while self.is_on.is_set():
            temperature, humidity = self.generate_sensor_data(current_temperature, current_humidity)
            self.client.publish(self.send_topic, json.dumps({"temperature": temperature, "humidity": humidity,
                                                             "consumptionPerMinute": round(self.power_per_hour / 60,
                                                                                           4)}), retain=False)
            await asyncio.sleep(60)
