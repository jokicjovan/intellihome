from Models.SmartDevice import SmartDevice
from datetime import datetime
import math
import asyncio
import json


# def generate_lumens():
#     current_time = datetime.now().time()
#     hours, minutes, seconds = current_time.hour, current_time.minute, current_time.second
#
#     lumens = 0
#
#     if 8 <= hours <= 18:
#         lumens = 1000
#     elif 19 <= hours <= 21 or 6 <= hours <= 7:
#         lumens = 500
#     elif 22 <= hours <= 23:
#         lumens = 100
#     elif 0 <= hours <= 5:
#         lumens = 0
#
#     return lumens


# nisam testirao ovu funkciju, malo ne radi
def generate_lumens():
    current_time = datetime.now().time()
    hours = current_time.hour + current_time.minute / 60

    time_rad = 2 * math.pi * hours / 24

    sine_val = math.sin(time_rad)

    adjusted_sine_val = (sine_val + 1) / 2

    lumens = adjusted_sine_val * 1000

    lumens = round(lumens, 2)
    return lumens


class Lamp(SmartDevice):
    def __init__(self, device_id, smart_home_id, device_category, device_type, brightness_limit, power_per_hour, is_auto, is_shining=False):
        super().__init__(device_id, smart_home_id, device_category, device_type)
        self.brightness_limit = brightness_limit
        self.power_per_hour = power_per_hour
        self.is_auto = is_auto
        self.is_shining = is_shining

    def on_data_receive(self, client, user_data, msg):
        super().on_data_receive(client, user_data, msg)
        if msg.topic == self.receive_topic:
            data = json.loads(msg.payload.decode())
            if data.get("action", None) == "auto":
                self.is_auto = True
            elif data.get("action", None) == "manual":
                self.is_auto = False
            elif data.get("action", None) == "set_brightness_limit":
                brightness_limit = data.get("brightness", None)
                self.brightness_limit = brightness_limit
            elif data.get("action", None) == "turn_lamp_on":
                self.is_shining = True
            elif data.get("action", None) == "turn_lamp_off":
                self.is_shining = False

    async def send_data(self):
        while True:
            if not self.is_on:
                break
            lumens = generate_lumens()
            print(f"Is auto: {self.is_auto}, lumens: {lumens}, brightness limit: {self.brightness_limit}, is shining: {self.is_shining}")
            if self.is_auto:
                self.is_shining = lumens < self.brightness_limit

            self.client.publish(self.send_topic, json.dumps({"currentBrightness": lumens,
                                                             "isShining": self.is_shining,
                                                             "isAuto": self.is_auto,
                                                             "consumptionPerMinute": round(self.power_per_hour / 60,
                                                                                                4)}), retain=False)
            await asyncio.sleep(10)
