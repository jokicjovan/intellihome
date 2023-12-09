from Models.SmartDevice import SmartDevice


class Sprinkler(SmartDevice):
    def __init__(self, device_id, smart_home_id, device_category, device_type, power_per_hour):
        super().__init__(device_id, smart_home_id, device_category, device_type)
        self.power_per_hour = power_per_hour
