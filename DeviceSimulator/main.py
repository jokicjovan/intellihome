import asyncio
from fastapi import FastAPI
from pydantic import BaseModel
import paho.mqtt.client as mqtt


class DeviceDTO(BaseModel):
    device_id: str
    smart_home_id: str
    host: str
    port: int
    keepalive: int


class Device:
    def __init__(self, device_id, smart_home_id):
        self.device_id = device_id
        self.smart_home_id = smart_home_id
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


class DevicesManager:
    def __init__(self):
        self.devices = {}

    def add_device(self, deviceDTO: DeviceDTO):
        device = Device(deviceDTO.device_id, deviceDTO.smart_home_id)
        device.connect(deviceDTO.host, deviceDTO.port, deviceDTO.keepalive)
        self.devices[device.device_id] = device

    def remove_device(self, device_id):
        device = self.devices.pop(device_id, None)
        if device:
            device.disconnect()


app = FastAPI()
devices_manager = DevicesManager()


@app.on_event("startup")
async def startup_event():
    pass


@app.post("/add-device/")
async def add_device(device: DeviceDTO):
    devices_manager.add_device(device)
    return {"message": f"Device added: {device.device_id}"}


@app.post("/remove-device/{device_id}")
async def remove_device(device_id: str):
    devices_manager.remove_device(device_id)
    return {"message": f"Device removed: {device_id}"}
