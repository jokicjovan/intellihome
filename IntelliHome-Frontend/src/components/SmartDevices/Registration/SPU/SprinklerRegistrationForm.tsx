import React, { useState } from 'react';
import SmartDeviceRegistrationForm from "../Shared/SmartDeviceRegistrationForm.tsx";
import {Box, Container, Typography} from "@mui/material";
import CommonSmartDeviceFields from "../../../../models/interfaces/CommonSmartDeviceFields.ts";
import SmartDeviceService from "../../../../services/smartDevices/SmartDeviceService.ts";
import SmartDeviceType from "../../../../models/enums/SmartDeviceType.ts";
import smartDeviceCategory from "../../../../models/enums/SmartDeviceCategory.ts";
import PowerPerHourInput from "../Shared/PowerPerHourInput.tsx";
import DeviceRegistrationButtons from "../Shared/DeviceRegistrationButtons.tsx";

interface SprinklerAdditionalFields {
    PowerPerHour: number;
}

interface SprinklerRegistrationFormProps {
    smartHomeId: string;
}

const SprinklerRegistrationForm : React.FC<SprinklerRegistrationFormProps> = ({smartHomeId}) => {
    const [additionalFormData, setAdditionalFormData] = useState<SprinklerAdditionalFields>({
        PowerPerHour: 1,
    });

    const [commonFormData, setCommonFormData] = useState<CommonSmartDeviceFields>({
        Name: "Sprinkler",
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
    const handleSprinklerSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        SmartDeviceService.registerSmartDevice({...commonFormData, ...additionalFormData}, smartHomeId, smartDeviceCategory.SPU, SmartDeviceType.Sprinkler);
    };

    return (
        <Container
            component="main"
            maxWidth="xs"
            sx={{
                display: "flex",
                flexDirection: "column",
                alignItems: "center",
                backgroundColor: "white",
                borderRadius: 3,
                justifyContent: "start"
            }}
        >
            <Box
                component="form"
                onSubmit={handleSprinklerSubmit}
                sx={{
                    display: "flex",
                    flexDirection: "column",
                    alignItems: "start",
                    width: 1,
                    margin: 2
                }}
            >
                <Typography variant="h4" sx={{ textAlign: "left", width: 1 }}>
                    Add Sprinkler
                </Typography>

                <SmartDeviceRegistrationForm
                    formData={commonFormData}
                    onFormChange={handleCommonFormInputChange}
                />

                <PowerPerHourInput
                    onValueChange={handlePowerValueChange}
                />

                <DeviceRegistrationButtons onCancel={() => {}}/>
            </Box>
        </Container>
    );
};

export default SprinklerRegistrationForm;