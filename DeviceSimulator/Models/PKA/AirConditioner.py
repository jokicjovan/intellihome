import json
import asyncio
from datetime import datetime

from Models.SmartDevice import SmartDevice


class AirConditioner(SmartDevice):
    def __init__(self, device_id, smart_home_id, device_category, device_type, schedule_list,
                 power_per_hour, current_mode='auto', current_temp=20):
        super().__init__(device_id, smart_home_id, device_category, device_type)
        self.current_mode = current_mode
        self.current_temp = current_temp
        self.power_per_hour = power_per_hour
        self.schedule_list = schedule_list

    def on_data_receive(self, client, user_data, msg):
        super().on_data_receive(client, user_data, msg)
        if msg.topic == self.receive_topic:
            data = json.loads(msg.payload.decode())
            if data.get("action", None) == "heat":
                self.current_mode = "heat"
            elif data.get("action", None) == "auto":
                self.current_mode = "auto"
            elif data.get("action", None) == "fan":
                self.current_mode = "fan"
            elif data.get("action", None) == "cool":
                self.current_mode = "cool"
            elif data.get("action", None) == "set_current_temperature":
                current_temp = data.get("temperature", None)
                self.current_temp = current_temp
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
