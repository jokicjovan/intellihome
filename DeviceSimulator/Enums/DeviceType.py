from enum import Enum


class DeviceType(str, Enum):
    AmbientSensor = "AmbientSensor",
    AirConditioner = "AirConditioner",
    WashingMachine = "WashingMachine",
    Lamp = "Lamp",
    VehicleGate = "VehicleGate",
    Sprinkler = "Sprinkler",
    BatterySystem = "BatterySystem",
    SolarPanelSystem = "SolarPanelSystem",
    VehicleCharger = "VehicleCharger"
