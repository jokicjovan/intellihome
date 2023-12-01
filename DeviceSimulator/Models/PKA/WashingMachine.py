from Models.SmartDevice import SmartDevice


class WashingMachine(SmartDevice):
    def __init__(self, device_id, smart_home_id, device_category, device_type, mode_name, mode_duration, mode_temp):
        super().__init__(device_id, smart_home_id, device_category, device_type)
        self.mode_name = mode_name
        self.mode_duration = mode_duration
        self.mode_temp = mode_temp
