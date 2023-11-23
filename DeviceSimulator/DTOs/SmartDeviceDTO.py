from pydantic import BaseModel


class SmartDeviceDTO(BaseModel):
    device_id: str
    smart_home_id: str
    device_type: str
    host: str
    port: int
    keepalive: int
