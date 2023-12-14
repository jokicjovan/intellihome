import SmartDeviceCategory from "../enums/SmartDeviceCategory.ts";
import SmartDeviceType from "../enums/SmartDeviceType.ts";

export default interface SmartDevice {
    id: number;
    name: string;
    image: string;
    category: SmartDeviceCategory;
    type: SmartDeviceType;
    isConnected: boolean;
    isOn: boolean;
    [key: string]: any;
}
