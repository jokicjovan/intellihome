import React, { useState } from 'react';
import SmartDeviceRegistrationForm from "../Shared/SmartDeviceRegistrationForm.tsx";
import {Box, Container, TextField, Typography} from "@mui/material";
import CommonSmartDeviceFields from "../../../../models/interfaces/CommonSmartDeviceFields.ts";
import InputAdornment from "@mui/material/InputAdornment";
import SmartDeviceService from "../../../../services/smartDevices/SmartDeviceService.ts";
import smartDeviceCategory from "../../../../models/enums/SmartDeviceCategory.ts";
import SmartDeviceType from "../../../../models/enums/SmartDeviceType.ts";
import PowerPerHourInput from "../Shared/PowerPerHourInput.tsx";
import DeviceRegistrationButtons from "../Shared/DeviceRegistrationButtons.tsx";
import {useMutation, useQueryClient} from "react-query";
import SmartDeviceCategory from "../../../../models/enums/SmartDeviceCategory.ts";
import {RotatingLines} from "react-loader-spinner";

interface LampAdditionalFields {
    PowerPerHour: number;
    BrightnessLimit: number;
}

interface LampRegistrationFormProps {
    smartHomeId: string;
    onClose: () => void;
}


const LampRegistrationForm : React.FC<LampRegistrationFormProps> = ({smartHomeId, onClose}) => {
    const queryClient = useQueryClient();
    const [isLoading, setIsLoading] = useState(false);
    const smartDeviceService = new SmartDeviceService();
    const [additionalFormData, setAdditionalFormData] = useState<LampAdditionalFields>({
        BrightnessLimit: 100,
        PowerPerHour: 1,
    });

    const [commonFormData, setCommonFormData] = useState<CommonSmartDeviceFields>({
        Name: "Lamp",
        Image: new Blob([])
    });

    const handlePowerValueChange = (powerValue: number) => {
        setAdditionalFormData((prevData) => ({
            ...prevData,
            PowerPerHour: powerValue
        }));
    };

    const handleCommonFormInputChange = (smartDeviceData: CommonSmartDeviceFields) => {
        setCommonFormData(smartDeviceData);
    };

    const handleAdditionalFormInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setAdditionalFormData({
            ...additionalFormData,
            [e.target.name]: e.target.value
        });
    };

    interface registrationMutationInterface {
        formData: any;
        smartHomeId: string;
        deviceCategory: SmartDeviceCategory;
        deviceType: SmartDeviceType;
    }

    const registrationMutation = useMutation(
        (params: registrationMutationInterface) => {
            const { formData, smartHomeId, deviceCategory, deviceType } = params;
            setIsLoading(true);
            return smartDeviceService.registerSmartDevice(formData, smartHomeId, deviceCategory, deviceType)
        },
        {
            onSuccess: (res) => {
                setIsLoading(false);
                if (res.status === 200) {
                    queryClient.invalidateQueries('smartDevicesForHome');
                    onClose();
                }
            },
            onError: (error) => {
                setIsLoading(false);
                console.error('Error:', error);
            },
        }
    );

    const handleLampSubmit = (e: React.FormEvent) => {
        e.preventDefault();

        const formData : any = {...commonFormData, ...additionalFormData}
        const payload = {formData : formData , smartHomeId : smartHomeId, deviceCategory : SmartDeviceCategory.SPU, deviceType: SmartDeviceType.LAMP}
        registrationMutation.mutate(payload);
    };

    return (<Box>{isLoading &&
        <Box sx={{position:"fixed", top:0, left:0, width:"100%", height:"100%", display:"flex", justifyContent:"center", alignItems:"center", zIndex:"9999", backgroundColor:"rgba(0,0,0,0.7)"}}>
            <RotatingLines
                visible={true}
                width="96"
                strokeWidth="5"
                animationDuration="0.75"
                ariaLabel="rotating-lines-loading"
                strokeColor={"#FBC40E"}/>
        </Box>}
        <Container
            maxWidth="xs"
            sx={{
                display: "flex",
                flexDirection: "column",
                alignItems: "center",
                backgroundColor: "white",
                borderRadius: 3,
                justifyContent: "start",
                padding:0,
                margin:0
            }}
        >
            <Box
                component="form"
                onSubmit={handleLampSubmit}
                sx={{
                    display: "flex",
                    flexDirection: "column",
                    alignItems: "start",
                    width: 1,
                    margin: 2
                }}
            >
                <Typography variant="h4" sx={{ textAlign: "left", width: 1 }}>
                    Add Lamp
                </Typography>

                <SmartDeviceRegistrationForm
                    formData={commonFormData}
                    onFormChange={handleCommonFormInputChange}
                />

                <PowerPerHourInput
                    onValueChange={handlePowerValueChange}
                />

                <TextField
                    variant="outlined"
                    margin="normal"
                    required
                    fullWidth
                    id="BrightnessLimit"
                    label="Brightness Limit"
                    name="BrightnessLimit"
                    type="number"
                    value={additionalFormData.BrightnessLimit}
                    onChange={handleAdditionalFormInputChange}
                    InputProps={{
                        endAdornment: <InputAdornment position="end">Lm</InputAdornment>,
                    }}
                    inputProps={{
                        min: 20,
                        max: 1500,
                    }}
                />

                <DeviceRegistrationButtons onCancel={onClose} onSubmit={() => {}}/>
            </Box>
        </Container>
    </Box>);
};

export default LampRegistrationForm;