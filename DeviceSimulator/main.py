from fastapi import FastAPI
from DTOs.SmartDeviceDTO import SmartDeviceDTO
from Models.SmartDeviceManager import SmartDeviceManager

app = FastAPI()
devices_manager = SmartDeviceManager()


@app.on_event("startup")
async def startup_event():
    pass


@app.post("/turn-on-device/{device_id}")
async def turn_on_device(device_id: str):
    devices_manager.turn_on_device(device_id)
    return {"message": f"Smart device turned on: {device_id}"}


@app.post("/turn-off-device/{device_id}")
async def turn_on_device(device_id: str):
    devices_manager.turn_off_device(device_id)
    return {"message": f"Smart device turned off: {device_id}"}


@app.post("/add-device")
async def add_device(smartDeviceDTO: SmartDeviceDTO):
    devices_manager.add_device(smartDeviceDTO)
    return {"message": f"Smart device added: {smartDeviceDTO.device_id}"}


@app.post("/remove-device/{device_id}")
async def remove_device(device_id: str):
    devices_manager.remove_device(device_id)
    return {"message": f"Smart device removed: {device_id}"}
