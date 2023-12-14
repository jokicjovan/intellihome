import React, { useState } from 'react';
import SmartDeviceRegistrationForm from "../Shared/SmartDeviceRegistrationForm.tsx";
import {Box, Container, TextField, Typography} from "@mui/material";
import CommonSmartDeviceFields from "../../../../models/interfaces/CommonSmartDeviceFields.ts";
import InputAdornment from "@mui/material/InputAdornment";
import SmartDeviceService from "../../../../services/smartDevices/SmartDeviceService.ts";
import smartDeviceCategory from "../../../../models/enums/SmartDeviceCategory.ts";
import SmartDeviceType from "../../../../models/enums/SmartDeviceType.ts";
import DeviceRegistrationButtons from "../Shared/DeviceRegistrationButtons.tsx";

interface SolarPanelSystemAdditionalFields {
    Area: number;
    Efficiency: number;
}

interface SolarPanelSystemRegistrationFormProps {
    smartHomeId: string;
    onClose: () => void;
}

const SolarPanelSystemRegistrationForm : React.FC<SolarPanelSystemRegistrationFormProps> = ({smartHomeId, onClose}) => {
    const smartDeviceService = new SmartDeviceService();
    const [additionalFormData, setAdditionalFormData] = useState<SolarPanelSystemAdditionalFields>({
        Area: 5,
        Efficiency: 1
    });

    const [commonFormData, setCommonFormData] = useState<CommonSmartDeviceFields>({
        Name: "SolarPanelSystem",
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

    const handleSolarPanelSystemSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        smartDeviceService.registerSmartDevice({...commonFormData, ...additionalFormData}, smartHomeId, smartDeviceCategory.VEU, SmartDeviceType.SOLARPANELSYSTEM)
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
                onSubmit={handleSolarPanelSystemSubmit}
                sx={{
                    display: "flex",
                    flexDirection: "column",
                    alignItems: "start",
                    width: 1,
                    margin: 2
                }}
            >
                <Typography variant="h4" sx={{ textAlign: "left", width: 1 }}>
                    Add Solar Panel System
                </Typography>

                <SmartDeviceRegistrationForm
                    formData={commonFormData}
                    onFormChange={handleCommonFormInputChange}
                />

                <Box sx={{display:"flex", width:1}}>
                <TextField
                    variant="outlined"
                    margin="normal"
                    required
                    fullWidth
                    id="Area"
                    label="Area"
                    name="Area"
                    type="number"
                    value={additionalFormData.Area}
                    onChange={handleAdditionalFormInputChange}
                    InputProps={{
                        endAdornment: <InputAdornment position="end">m^2</InputAdornment>,
                    }}
                    inputProps={{
                        min: 5,
                        max: 10000,
                    }}
                    sx={{marginRight:1}}
                />

                <TextField
                    variant="outlined"
                    margin="normal"
                    required
                    fullWidth
                    id="Efficiency"
                    label="Efficiency"
                    name="Efficiency"
                    type="number"
                    value={additionalFormData.Efficiency}
                    onChange={handleAdditionalFormInputChange}
                    InputProps={{
                        endAdornment: <InputAdornment position="end">%</InputAdornment>,
                    }}
                    inputProps={{
                        min: 1,
                        max: 100,
                    }}
                    sx={{marginLeft:1}}
                />
                </Box>

                <DeviceRegistrationButtons onCancel={onClose} onSubmit={() => {}}/>
            </Box>
        </Container>
    );
};

export default SolarPanelSystemRegistrationForm;