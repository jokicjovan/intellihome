enum SmartDeviceType {
    AirConditioner = "AirConditioner",
    AIRCONDITIONER = "AirConditioner",
    AmbientSensor = "AmbientSensor",
    AMBIENTSENSOR = "AmbientSensor",
    WashingMachine = "WashingMachine",
    WASHINGMACHINE = "WashingMachine",
    Lamp = "Lamp",
    LAMP = "Lamp",
    Sprinkler = "Sprinkler",
    SPRINKLER = "Sprinkler",
    VehicleGate = "VehicleGate",
    VEHICLEGATE = "VehicleGate",
    SolarPanelSystem = "SolarPanelSystem",
    SOLARPANELSYSTEM = "SolarPanelSystem",
    BatterySystem = "BatterySystem",
    BATTERYSYSTEM = "BatterySystem",
    VehicleCharger = "VehicleCharger",
    VEHICLECHARGER = "VehicleCharger",
}

export default  SmartDeviceType

function getSmartDeviceTypeValueByKey(key: string): SmartDeviceType | undefined {
    const enumValues = Object.keys(SmartDeviceType);
    return SmartDeviceType[key as SmartDeviceType];
}

export {
    getSmartDeviceTypeValueByKey
}