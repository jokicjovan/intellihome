import React, { useState } from 'react';
import SmartDeviceRegistrationForm from "../Shared/SmartDeviceRegistrationForm.tsx";
import {Box, Button, Checkbox, Container, FormControlLabel, TextField, Typography} from "@mui/material";
import CommonSmartDeviceFields from "../Shared/CommonSmartDeviceFields.ts";
import InputAdornment from "@mui/material/InputAdornment";
import axios from "axios";
import {environment} from "../../../../security/Environment.tsx";

interface AirConditionerAdditionalFields {
    Modes: string[];
    MinTemperature: number;
    MaxTemperature: number;
}

interface AirConditionerRegistrationFormProps {
    smartHomeId: string;
}


const AirConditionerRegistrationForm : React.FC<AirConditionerRegistrationFormProps> = ({smartHomeId}) => {
    const [additionalFormData, setAdditionalFormData] = useState<AirConditionerAdditionalFields>({
        Modes: [],
        MinTemperature: 15,
        MaxTemperature: 35
    });

    const [commonFormData, setCommonFormData] = useState<CommonSmartDeviceFields>({
        PowerPerHour: 0,
        Name: "Air Conditioner",
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
        axios.post(
            `${environment}/api/PKA/CreateAirConditioner`,
            {...commonFormData, ...additionalFormData},
            {
                params: { smartHomeId: smartHomeId },
                headers: {
                    'Content-Type': 'multipart/form-data',
                },
            }
        )
            .then((res) => {
                if (res.status === 200) console.log("Success");
            })
            .catch((error) => {
                console.error("Error:", error);
            });
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

                <TextField
                    variant="outlined"
                    margin="normal"
                    required
                    fullWidth
                    id="MinTemperature"
                    label="Minimum temperature"
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
                    label="Maximum temperature"
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

                <Box
                    sx={{
                        display: "flex",
                        justifyContent: "end",
                        margin: 1,
                        marginBottom: 0,
                        width: 1,
                    }}
                >
                    <Button
                        variant="contained"
                        color="primary"
                        sx={{
                            marginRight: 2,
                            backgroundColor: "white",
                            border: 1,
                            '&:hover': {
                                backgroundColor: 'lightGray',
                            },
                        }}
                    >
                        Cancel
                    </Button>
                    <Button
                        type="submit"
                        variant="contained"
                        color="primary"
                        sx={{
                            border: 1,
                        }}
                    >
                        Create
                    </Button>
                </Box>
            </Box>
        </Container>
    );
};

export default AirConditionerRegistrationForm;