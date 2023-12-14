import React, { useState } from 'react';
import SmartDeviceRegistrationForm from "../Shared/SmartDeviceRegistrationForm.tsx";
import {Box, Container, TextField, Typography} from "@mui/material";
import CommonSmartDeviceFields from "../../../../models/interfaces/CommonSmartDeviceFields.ts";
import InputAdornment from "@mui/material/InputAdornment";
import SmartDeviceService from "../../../../services/smartDevices/SmartDeviceService.ts";
import smartDeviceCategory from "../../../../models/enums/SmartDeviceCategory.ts";
import SmartDeviceType from "../../../../models/enums/SmartDeviceType.ts";
import DeviceRegistrationButtons from "../Shared/DeviceRegistrationButtons.tsx";

interface BatterySystemAdditionalFields {
    Capacity: number;
}

interface BatterySystemRegistrationFormProps {
    smartHomeId: string;
    onClose: () => void;
}

const BatterySystemRegistrationForm : React.FC<BatterySystemRegistrationFormProps> = ({smartHomeId, onClose}) => {
    const smartDeviceService = new SmartDeviceService();
    const [additionalFormData, setAdditionalFormData] = useState<BatterySystemAdditionalFields>({
        Capacity: 10,
    });

    const [commonFormData, setCommonFormData] = useState<CommonSmartDeviceFields>({
        Name: "BatterySystem",
        Image: new Blob([])
    });

    const handleCommonFormInputChange = (smartDeviceData: CommonSmartDeviceFields) => {
        setCommonFormData(smartDeviceData);
    };

    const handleAdditionalFormInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setAdditionalFormData({
            ...additionalFormData,
            [e.target.name]: e.target.value
        });
    };

    const handleBatterySystemSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        smartDeviceService.registerSmartDevice({...commonFormData, ...additionalFormData}, smartHomeId, smartDeviceCategory.VEU, SmartDeviceType.BATTERYSYSTEM)
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
                onSubmit={handleBatterySystemSubmit}
                sx={{
                    display: "flex",
                    flexDirection: "column",
                    alignItems: "start",
                    width: 1,
                    margin: 2
                }}
            >
                <Typography variant="h4" sx={{ textAlign: "left", width: 1 }}>
                    Add Battery System
                </Typography>

                <SmartDeviceRegistrationForm
                    formData={commonFormData}
                    onFormChange={handleCommonFormInputChange}
                />

                <TextField
                    variant="outlined"
                    margin="normal"
                    required
                    fullWidth
                    id="Capacity"
                    label="Capacity"
                    name="Capacity"
                    type="number"
                    value={additionalFormData.Capacity}
                    onChange={handleAdditionalFormInputChange}
                    InputProps={{
                        endAdornment: <InputAdornment position="end">KWh</InputAdornment>,
                    }}
                    inputProps={{
                        min: 10,
                        max: 1000,
                    }}
                />

                <DeviceRegistrationButtons onCancel={onClose} onSubmit={() => {}}/>
            </Box>
        </Container>
    );
};

export default BatterySystemRegistrationForm;