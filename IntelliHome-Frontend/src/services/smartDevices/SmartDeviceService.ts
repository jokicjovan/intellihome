import axios, {AxiosResponse} from 'axios';
import SmartDeviceType from "../../models/enums/SmartDeviceType.ts";
import SmartDeviceCategory from "../../models/enums/SmartDeviceCategory.ts";
import {environment} from "../../utils/Environment.ts";

class SmartDeviceService {

    constructor() {
    }

    public registerSmartDevice(
        formData: any,
        smartHomeId: string,
        smartDeviceCategory: SmartDeviceCategory,
        smartDeviceType: SmartDeviceType
    ): Promise<AxiosResponse<any>> {
        const url = `${environment}/api/${smartDeviceCategory}/Create${smartDeviceType}/${smartHomeId}`;
        try {
            return axios.post(url, formData, {
                headers: {
                    'Content-Type': 'multipart/form-data',
                },
            });
        } catch (error) {
            throw error;
        }
    }
}

export default SmartDeviceService;