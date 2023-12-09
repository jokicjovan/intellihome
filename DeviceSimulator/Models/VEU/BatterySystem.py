import asyncio
import json

from Models.SmartDevice import SmartDevice


class BatterySystem(SmartDevice):
    def __init__(self, device_id, smart_home_id, device_category, device_type, capacity):
        super().__init__(device_id, smart_home_id, device_category, device_type)
        self.capacity = capacity
        self.current_capacity = 0
        self.current_production = 0
        self.current_consumption = 0
        self.lock = asyncio.Lock()

    async def send_data(self):
        while True:
            if not self.is_on:
                break
            async with self.lock:
                difference = self.current_production - self.current_consumption
                grid = 0
                if self.current_capacity + difference > self.capacity:
                    grid = self.current_capacity + difference - self.capacity
                    self.current_capacity = self.capacity
                elif self.current_capacity + difference < 0:
                    grid = self.current_capacity + difference
                    self.current_capacity = 0
                else:
                    self.current_capacity += difference
                print({"current_capacity": self.current_capacity,
                                                                 "current_consumption": self.current_consumption,
                                                                 "grid": grid
                                                                 })
                self.client.publish(self.send_topic, json.dumps({"current_capacity": self.current_capacity,
                                                                 "current_consumption": self.current_consumption,
                                                                 "grid": grid
                                                                 }), retain=False)
                self.current_production = 0
                self.current_consumption = 0
                await asyncio.sleep(10)

