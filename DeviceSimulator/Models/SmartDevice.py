import asyncio
import paho.mqtt.client as mqtt


class SmartDevice:
    def __init__(self, device_id, smart_home_id, device_category):
        self.device_id = device_id
        self.smart_home_id = smart_home_id
        self.device_category = device_category
        self.event = asyncio.Event()
        self.client = mqtt.Client(client_id=device_id, clean_session=True)

    async def send_data(self):
        while True:
            await asyncio.sleep(5)

    def connect(self, host, port, keepalive):
        self.client.will_set("will", payload=f"{self.device_id}", qos=1, retain=False)
        self.client.connect(host, port, keepalive=keepalive)
        self.client.loop_start()

    def disconnect(self):
        self.client.loop_stop()
