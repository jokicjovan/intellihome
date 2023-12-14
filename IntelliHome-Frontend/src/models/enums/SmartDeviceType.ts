enum SmartDeviceType {
    AIRCONDITIONER = "AirConditioner",
    AMBIENTSENSOR = "AmbientSensor",
    WASHINGMACHINE = "WashingMachine",
    LAMP = "Lamp",
    SPRINKLER = "Sprinkler",
    VEHICLEGATE = "VehicleGate",
    SOLARPANELSYSTEM = "SolarPanelSystem",
    BATTERYSYSTEM = "BatterySystem",
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