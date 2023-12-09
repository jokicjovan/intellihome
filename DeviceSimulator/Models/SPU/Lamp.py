from Models.SmartDevice import SmartDevice
from datetime import datetime
import math
import asyncio
import json


def generate_lumens():
    current_time = datetime.now().time()
    hours, minutes, seconds = current_time.hour, current_time.minute, current_time.second

    lumens = 0

    if 6 <= hours <= 18:
        lumens = 1000
    elif 19 <= hours <= 21:
        lumens = 500
    elif 22 <= hours <= 23:
        lumens = 100
    elif 0 <= hours <= 5:
        lumens = 0

    return lumens


class Lamp(SmartDevice):
    def __init__(self, device_id, smart_home_id, device_category, device_type, brightness_limit, power_per_hour, is_auto):
        super().__init__(device_id, smart_home_id, device_category, device_type)
        self.brightness_limit = brightness_limit
        self.power_per_hour = power_per_hour
        self.is_auto = is_auto


    async def send_data(self):
        while True:
            if not self.is_on:
                break

            lumens = generate_lumens()

            if self.is_auto:
                is_working = lumens < self.brightness_limit
            else:
                is_working = self.is_on

            self.client.publish(self.send_topic, json.dumps({"CurrentBrightness": lumens,
                                                             "IsWorking": is_working,
                                                             "ConsumptionPerMinute": round(self.power_per_hour / 60,
                                                                                                4)}), retain=False)
            await asyncio.sleep(10)
