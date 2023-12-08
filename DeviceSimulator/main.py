from fastapi import FastAPI
from DTOs.SmartDeviceDTO import SmartDeviceDTO
from DTOs.SmartDeviceShortDTO import SmartDeviceShortDTO
from Models.SmartHomeManager import SmartHomeManager

app = FastAPI()
smart_home_manager = SmartHomeManager()


@app.on_event("startup")
async def startup_event():
    pass


@app.post("/turn-on-device")
async def turn_on_device(device_short_dto: SmartDeviceShortDTO):
    smart_home_manager.turn_on_device_in_home(device_short_dto)
    return {"message": f"Smart device turned on: {device_short_dto.device_id}"}


@app.post("/turn-off-device")
async def turn_on_device(device_short_dto: SmartDeviceShortDTO):
    smart_home_manager.turn_off_device_in_home(device_short_dto)
    return {"message": f"Smart device turned off: {device_short_dto.device_id}"}


@app.post("/add-device")
async def add_device(device_dto: SmartDeviceDTO):
    smart_home_manager.add_device_to_home(device_dto)
    return {"message": f"Smart device added: {device_dto.device_id}"}


@app.post("/remove-device")
async def remove_device(device_short_dto: SmartDeviceShortDTO):
    smart_home_manager.remove_device_from_home(device_short_dto)
    return {"message": f"Smart device removed: {device_short_dto.device_id}"}
