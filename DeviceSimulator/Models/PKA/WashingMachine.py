import asyncio
import json
from datetime import datetime

from Models.SmartDevice import SmartDevice


class WashingMachine(SmartDevice):
    def __init__(self, device_id, smart_home_id, device_category, device_type,schedule_list,
                 power_per_hour,mode_name='mixed wash', mode_temp=30):
        super().__init__(device_id, smart_home_id, device_category, device_type)
        self.current_mode = mode_name
        self.current_temp = mode_temp
        self.power_per_hour = power_per_hour
        self.schedule_list = schedule_list

    def on_data_receive(self, client, user_data, msg):
        super().on_data_receive(client, user_data, msg)
        if msg.topic == self.receive_topic:
            data = json.loads(msg.payload.decode())
            if data.get("action", None) == "mixed wash":
                self.current_temp = data.get("temperature", None)
                self.current_mode = "mixed wash"
            elif data.get("action", None) == "antiallergy":
                self.current_temp = data.get("temperature", None)
                self.current_mode = "antiallergy"
            elif data.get("action", None) == "white wash":
                self.current_temp = data.get("temperature", None)
                self.current_mode = "white wash"
            elif data.get("action", None) == "add_schedule":
                current_temp = data.get("temperature", None)
                current_mode = data.get("mode", None)
                current_timestamp = data.get("timestamp", None)
                self.schedule_list.append(
                    {"timestamp": current_timestamp, "temperature": current_temp, "mode": current_mode})

    async def send_data(self):
        while self.is_on.is_set():
            for item in self.schedule_list:
                if datetime.strptime(item['timestamp'], '%d/%m/%Y %H:%M') <= datetime.utcnow():
                    if item['mode'] == 'turn_off':
                        self.schedule_list.remove(item)
                        await self.turn_off()
                    else:
                        self.current_mode = item['mode']
                        self.current_temp = item['temperature']
                        self.schedule_list.remove(item)
            self.client.publish(self.send_topic, json.dumps(
                {"mode": self.current_mode, "temperature": self.current_temp,
                 "consumptionPerMinute": round(self.power_per_hour / 60, 4)}),
                                retain=False)
            print({"mode": self.current_mode, "temperature": self.current_temp,
                   "consumptionPerMinute": round(self.power_per_hour / 60, 4), "scheduledTasks": self.schedule_list})

            await asyncio.sleep(10)
