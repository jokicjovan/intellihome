import asyncio
import paho.mqtt.client as mqtt


class SmartDevice:
    def __init__(self, device_id, smart_home_id, device_category, device_type):
        self.device_id = device_id
        self.smart_home_id = smart_home_id
        self.device_category = device_category
        self.device_type = device_type
        self.is_on = False
        self.client = mqtt.Client(client_id=device_id, clean_session=True)
        self.send_topic = (
            f"FromDevice/{self.smart_home_id}/{self.device_category.value}/{self.device_type.value}/{self.device_id}")
        self.receive_topic = (
            f"ToDevice/{self.smart_home_id}/{self.device_category.value}/{self.device_type.value}/{self.device_id}")

    async def send_data(self):
        pass

    def on_data_receive(self, client, user_data, msg):
        pass

    def on_device_connect(self, client, userdata, flags, rc):
        self.client.subscribe(topic=self.receive_topic)

    def setup_connection(self, host, port, keepalive):
        self.client.will_set("will", payload=f"{self.device_id}", qos=1, retain=False)
        self.client.on_message = self.on_data_receive
        self.client.on_connect = self.on_device_connect
        self.client.connect(host, port, keepalive=keepalive)
        self.client.loop_start()

    def turn_on(self):
        self.is_on = True
        asyncio.create_task(self.send_data())

    def turn_off(self):
        self.is_on = False

    def disconnect(self):
        self.client.loop_stop()
