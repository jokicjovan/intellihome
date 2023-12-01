from Models.SmartDevice import SmartDevice


class SolarPanelSystem(SmartDevice):
    def __init__(self, device_id, smart_home_id, device_category, device_type, area, efficiency):
        super().__init__(device_id, smart_home_id, device_category, device_type)
        self.area = area
        self.efficiency = efficiency
