from Models.SmartDevice import SmartDevice


class Lamp(SmartDevice):
    def __init__(self, device_id, smart_home_id, device_category, device_type, brightness_limit):
        super().__init__(device_id, smart_home_id, device_category, device_type)
        self.brightness_limit = brightness_limit
