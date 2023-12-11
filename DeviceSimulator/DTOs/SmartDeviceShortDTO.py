from pydantic import BaseModel


class SmartDeviceShortDTO(BaseModel):
    device_id: str
    smart_home_id: str
