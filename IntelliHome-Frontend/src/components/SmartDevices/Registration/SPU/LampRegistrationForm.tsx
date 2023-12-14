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

interface LampAdditionalFields {
    PowerPerHour: number;
    BrightnessLimit: number;
}

interface LampRegistrationFormProps {
    smartHomeId: string;
    onClose: () => void;
}


const LampRegistrationForm : React.FC<LampRegistrationFormProps> = ({smartHomeId, onClose}) => {
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

    const handleLampSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        smartDeviceService.registerSmartDevice({...commonFormData, ...additionalFormData}, smartHomeId, smartDeviceCategory.SPU, SmartDeviceType.LAMP)
            .then((res) => {
                if (res.status === 200) {
                    onClose();
                }
            })
            .catch((error) => {
                console.error("Error:", error);
            });
    };

    return (
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
    );
};

export default LampRegistrationForm;