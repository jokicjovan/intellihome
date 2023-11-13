import asyncio
from fastapi import FastAPI
import paho.mqtt.client as mqtt


class Device:
    def __init__(self, device_id):
        self.device_id = device_id
        self.event = asyncio.Event()
        self.client = mqtt.Client(client_id=device_id, clean_session=True)
        self.client.will_set("will", payload=f"{device_id}", qos=1, retain=False)
        self.client.connect("localhost", 1883, keepalive=300)
        self.client.loop_start()

    async def send_data(self):
        while True:
            await asyncio.sleep(5)


class DevicesManager:
    def __init__(self):
        self.devices = {}

    def add_device(self, device_id):
        device = Device(device_id)
        self.devices[device_id] = device
        asyncio.create_task(device.send_data())

    def remove_device(self, device_id):
        self.devices.get(device_id)
        device = self.devices.pop(device_id, None)
        if device:
            device.client.disconnect()


app = FastAPI()
devices_manager = DevicesManager()


@app.on_event("startup")
async def startup_event():
    pass


@app.post("/add-device/{device_id}")
async def add_device(device_id: str):
    devices_manager.add_device(device_id)
    return {"message": f"Device added: {device_id}"}


@app.post("/remove-device/{device_id}")
async def remove_device(device_id: str):
    devices_manager.remove_device(device_id)
    return {"message": f"Device removed: {device_id}"}
