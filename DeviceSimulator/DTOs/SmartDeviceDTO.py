from pydantic import BaseModel

from Enums.DeviceCategory import DeviceCategory
from Enums.DeviceType import DeviceType


class SmartDeviceDTO(BaseModel):
    device_id: str
    smart_home_id: str
    device_category: DeviceCategory
    device_type: DeviceType
    host: str
    port: int
    keepalive: int
    kwargs: dict = {}
