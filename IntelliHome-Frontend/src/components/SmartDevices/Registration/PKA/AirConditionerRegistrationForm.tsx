import React, { useState } from 'react';
import SmartDeviceRegistrationForm from "../Shared/SmartDeviceRegistrationForm.tsx";
import {
    Box,
    Checkbox,
    Container,
    FormControlLabel,
    TextField,
    Typography
} from "@mui/material";
import CommonSmartDeviceFields from "../../../../models/interfaces/CommonSmartDeviceFields.ts";
import InputAdornment from "@mui/material/InputAdornment";
import SmartDeviceService from "../../../../services/smartDevices/SmartDeviceService.ts";
import SmartDeviceType from "../../../../models/enums/SmartDeviceType.ts";
import smartDeviceCategory from "../../../../models/enums/SmartDeviceCategory.ts";
import PowerPerHourInput from "../Shared/PowerPerHourInput.tsx";
import DeviceRegistrationButtons from "../Shared/DeviceRegistrationButtons.tsx";

interface AirConditionerAdditionalFields {
    PowerPerHour: number;
    Modes: string[];
    MinTemperature: number;
    MaxTemperature: number;
}

interface AirConditionerRegistrationFormProps {
    smartHomeId: string;
}


const AirConditionerRegistrationForm : React.FC<AirConditionerRegistrationFormProps> = ({smartHomeId}) => {
    const [additionalFormData, setAdditionalFormData] = useState<AirConditionerAdditionalFields>({
        PowerPerHour: 1,
        Modes: [],
        MinTemperature: 15,
        MaxTemperature: 30
    });

    const [commonFormData, setCommonFormData] = useState<CommonSmartDeviceFields>({
        Name: "Air Conditioner",
        Image: new Blob([])
    });

    const handlePowerValueChange = (powerValue: number) => {
        setAdditionalFormData((prevData) => ({
            ...prevData,
            PowerPerHour: powerValue
        }));
    };

    const handleAdditionalFormInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setAdditionalFormData({
            ...additionalFormData,
            [e.target.name]: e.target.value
        });
    };

    const handleCommonFormInputChange = (smartDeviceData: CommonSmartDeviceFields) => {
        setCommonFormData(smartDeviceData);
    };

    const handleCheckboxChange = (mode: string) => {
        const isSelected = additionalFormData.Modes.includes(mode);
        setAdditionalFormData((prevData) => ({
            ...prevData,
            Modes: isSelected
                ? prevData.Modes.filter((selectedMode) => selectedMode !== mode)
                : [...prevData.Modes, mode],
        }));
    };

    const handleAirConditionerSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        SmartDeviceService.registerSmartDevice({...commonFormData, ...additionalFormData}, smartHomeId, smartDeviceCategory.PKA, SmartDeviceType.AirConditioner);
    };

    const modesCheckboxes = [
        { display: "Cool", value: "0" },
        { display: "Heat", value: "1" },
        { display: "Fan", value: "2" },
        { display: "Auto", value: "3" },
    ];

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
                onSubmit={handleAirConditionerSubmit}
                sx={{
                    display: "flex",
                    flexDirection: "column",
                    alignItems: "start",
                    width: 1,
                    margin: 2
                }}
            >
                <Typography variant="h4" sx={{ textAlign: "left", width: 1 }}>
                    Add Air Conditioner
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
                    id="MinTemperature"
                    label="Minimum Temperature"
                    name="MinTemperature"
                    type="number"
                    value={additionalFormData.MinTemperature}
                    onChange={handleAdditionalFormInputChange}
                    InputProps={{
                        endAdornment: <InputAdornment position="end">°C</InputAdornment>,
                    }}
                    inputProps={{
                        min: 10,
                        max: 20,
                    }}
                />

                <TextField
                    variant="outlined"
                    margin="normal"
                    required
                    fullWidth
                    id="MaxTemperature"
                    label="Maximum Temperature"
                    name="MaxTemperature"
                    type="number"
                    value={additionalFormData.MaxTemperature}
                    onChange={handleAdditionalFormInputChange}
                    InputProps={{
                        endAdornment: <InputAdornment position="end">°C</InputAdornment>,
                    }}
                    inputProps={{
                        min: 25,
                        max: 35,
                    }}
                />

                <Box sx={{width:1}}>
                    <Typography variant="h6" sx={{ textAlign: "left", width: 1 }}>
                        Modes:
                    </Typography>
                    {modesCheckboxes.map((option) => (
                        <FormControlLabel
                            key={option.value}
                            control={
                                <Checkbox
                                    checked={additionalFormData.Modes.includes(option.value)}
                                    onChange={() => handleCheckboxChange(option.value)}
                                />
                            }
                            label={option.display}
                        />
                    ))}
                </Box>

                <DeviceRegistrationButtons onCancel={() => {}}/>
            </Box>
        </Container>
    );
};

export default AirConditionerRegistrationForm;