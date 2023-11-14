import React, {useEffect, useState} from 'react';
import SmartDeviceRegistrationForm from "../Shared/SmartDeviceRegistrationForm.tsx";
import {Box, Button, Checkbox, Container, FormControlLabel, Typography} from "@mui/material";
import CommonSmartDeviceFields from "../Shared/CommonSmartDeviceFields.ts";
import axios from "axios";
import {environment} from "../../../../security/Environment.tsx";

interface WashingMachineAdditionalFields {
    ModesIds: string[];
}

interface WashingMachineRegistrationFormProps {
    smartHomeId: string;
}

interface WashingMachineMode{
    id: string;
    name: string;
    duration: number;
    temperature: number;
}

interface ModeCheckbox{
    display: string;
    value: string;
}


const WashingMachineRegistrationForm : React.FC<WashingMachineRegistrationFormProps> = ({smartHomeId}) => {
    const [additionalFormData, setAdditionalFormData] = useState<WashingMachineAdditionalFields>({
        ModesIds: [],
    });

    const [commonFormData, setCommonFormData] = useState<CommonSmartDeviceFields>({
        PowerPerHour: 0,
        Name: "Washing Machine",
        Image: new Blob()
    });

    const handleCommonFormInputChange = (smartDeviceData: CommonSmartDeviceFields) => {
        setCommonFormData(smartDeviceData);
    };

    const handleCheckboxChange = (mode: string) => {
        const isSelected = additionalFormData.ModesIds.includes(mode);

        setAdditionalFormData((prevData) => ({
            ...prevData,
            ModesIds: isSelected
                ? prevData.ModesIds.filter((selectedMode) => selectedMode !== mode)
                : [...prevData.ModesIds, mode],
        }));
    };

    const handleAirConditionerSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        axios.post(
            `${environment}/api/PKA/CreateWashingMachine`,
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

    const [modesCheckboxes , setModesCheckboxes] = useState<ModeCheckbox[]>([]);


    useEffect(() => {
        axios.get(
            `${environment}/api/PKA/GetWashingMachineModes`,
        )
            .then((res) => {
                if (res.status === 200){
                    const modes : ModeCheckbox[] = []
                    res.data.forEach((mode: WashingMachineMode) => {
                        modes.push({ display: `${mode.name} (${mode.duration} min) (${mode.temperature}°C)`, value: mode.id })
                    });
                    setModesCheckboxes(modes);
                }
            })
            .catch((error) => {
                console.error("Error:", error);
            });
    })

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
                    Add Washing Machine
                </Typography>

                <SmartDeviceRegistrationForm
                    formData={commonFormData}
                    onFormChange={handleCommonFormInputChange}
                />

                <Box sx={{width:1}}>
                    <Typography variant="h6" sx={{ textAlign: "left", width: 1 }}>
                        Modes:
                    </Typography>
                    {modesCheckboxes.map((option : ModeCheckbox) => (
                        <FormControlLabel
                            key={option.value}
                            control={
                                <Checkbox
                                    checked={additionalFormData.ModesIds.includes(option.value)}
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

export default WashingMachineRegistrationForm;