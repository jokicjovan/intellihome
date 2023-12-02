from Models.SmartDevice import SmartDevice


class VehicleCharger(SmartDevice):
    def __init__(self, device_id, smart_home_id, device_category, device_type):
        super().__init__(device_id, smart_home_id, device_category, device_type)
