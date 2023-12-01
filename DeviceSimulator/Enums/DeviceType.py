from enum import Enum


class DeviceType(str, Enum):
    AmbientSensor = "AMBIENTSENSOR",
    AirConditioner = "AIRCONDITIONER",
    WashingMachine = "WASHINGMACHINE",
    Lamp = "LAMP",
    VehicleGate = "VEHICLEGATE",
    Sprinkler = "SPRINKLER",
    BatterySystem = "BATTERYSYSTEM",
    SolarPanelSystem = "SOLARPANELSYSTEM",
    VehicleCharger = "VEHICLECHARGER"
