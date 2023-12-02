from Models.SmartDevice import SmartDevice


class AirConditioner(SmartDevice):
    def __init__(self, device_id, smart_home_id, device_category, device_type, current_mode, current_temp):
        super().__init__(device_id, smart_home_id, device_category, device_type)
        self.current_mode = current_mode
        self.current_temp = current_temp
