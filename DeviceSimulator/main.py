from fastapi import FastAPI

from DTOs.SmartDeviceDTO import SmartDeviceDTO
from Models.DeviceManager import DeviceManager

app = FastAPI()
devices_manager = DeviceManager()


@app.on_event("startup")
async def startup_event():
    pass


@app.post("/add-device/")
async def add_device(device: SmartDeviceDTO):
    devices_manager.add_device(device)
    return {"message": f"Device added: {device.device_id}"}


@app.post("/remove-device/{device_id}")
async def remove_device(device_id: str):
    devices_manager.remove_device(device_id)
    return {"message": f"Device removed: {device_id}"}
