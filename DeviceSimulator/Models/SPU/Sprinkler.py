from Models.SmartDevice import SmartDevice
import json
import asyncio
from datetime import datetime

class Sprinkler(SmartDevice):
    def __init__(self, device_id, smart_home_id, device_category, device_type, power_per_hour, is_spraying=False,
                 schedule_list=None):
        super().__init__(device_id, smart_home_id, device_category, device_type)
        if schedule_list is None:
            schedule_list = []
        self.power_per_hour = power_per_hour
        self.is_spraying = is_spraying
        self.schedule_list = schedule_list

    def on_data_receive(self, client, user_data, msg):
        super().on_data_receive(client, user_data, msg)
        if msg.topic == self.receive_topic:
            data = json.loads(msg.payload.decode())
            if data.get("action", None) == "turn_sprinkler_on":
                self.is_spraying = True
            elif data.get("action", None) == "turn_sprinkler_off":
                self.is_spraying = False
            elif data.get("action", None) == "add_schedule":
                set_spraying = data.get("set_spraying", None)
                current_timestamp = data.get("timestamp", None)
                self.schedule_list.append({"timestamp": current_timestamp, "set_spraying": set_spraying})

    async def send_data(self):
        # print("send_data")
        while self.is_on.is_set():
            for item in self.schedule_list:
                if datetime.strptime(item['timestamp'], '%d/%m/%Y %H:%M') <= datetime.utcnow():
                    self.is_spraying = item['set_spraying']
                    self.schedule_list.remove(item)

            self.client.publish(self.send_topic, json.dumps(
                {"isSpraying": self.is_spraying, "consumptionPerMinute": round(self.power_per_hour / 60, 4), "scheduledTasks": self.schedule_list}),
                                retain=False)
            # print({"isSpraying": self.is_spraying, "consumptionPerMinute": round(self.power_per_hour / 60, 4),
            #        "scheduledTasks": self.schedule_list})
            await asyncio.sleep(60)
