import asyncio
from Models.SmartDevice import SmartDevice


class BatterySystem(SmartDevice):
    def __init__(self, device_id, smart_home_id, device_category, device_type, capacity):
        super().__init__(device_id, smart_home_id, device_category, device_type)
        self.capacity = capacity
        self.current_capacity = 0
        self.grid = 0

    async def send_data(self):
        while True:
            self.grid = 0
            await asyncio.sleep(10)
