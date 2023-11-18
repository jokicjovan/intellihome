import axios from 'axios';
import {environment} from "../../security/Environment.tsx";
import SmartDeviceType from "../../models/enums/SmartDeviceType.ts";
import SmartDeviceCategory from "../../models/enums/SmartDeviceCategory.ts";

const SmartDeviceService = {
    registerSmartDevice: async (formData : any, smartHomeId : string, smartDeviceCategory: SmartDeviceCategory, smartDeviceType: SmartDeviceType) => {
        return axios.post(
            `${environment}/api/${smartDeviceCategory}/Create${smartDeviceType}/${smartHomeId}`,
            formData,
            {
                headers: {
                    'Content-Type': 'multipart/form-data',
                },
            }
        )
    },
};

export default SmartDeviceService;