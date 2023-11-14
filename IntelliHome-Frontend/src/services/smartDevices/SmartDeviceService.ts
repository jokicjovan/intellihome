import axios from 'axios';
import {environment} from "../../security/Environment.tsx";
import SmartDeviceType from "../../models/enums/SmartDeviceType.ts";
import SmartDeviceCategory from "../../models/enums/SmartDeviceCategory.ts";

const SmartDeviceService = {
    registerSmartDevice: async (formData : any, smartHomeId : string, smartDeviceCategory: SmartDeviceCategory, smartDeviceType: SmartDeviceType) => {
        axios.post(
            `${environment}/api/${smartDeviceCategory}/Create${smartDeviceType}`,
            formData,
            {
                params: { smartHomeId: smartHomeId },
                headers: {
                    'Content-Type': 'multipart/form-data',
                },
            }
        )
            .then((res) => {
                if (res.status === 200) {
                    console.log("Success");
                }
            })
            .catch((error) => {
                console.error("Error:", error);
            });
    },
};

export default SmartDeviceService;