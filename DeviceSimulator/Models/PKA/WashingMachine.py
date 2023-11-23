from Models.SmartDevice import SmartDevice


class WashingMachine(SmartDevice):
    def __init__(self, device_id, smart_home_id, device_category, modes):
        super().__init__(device_id, smart_home_id, device_category)
        self.modes = modes
