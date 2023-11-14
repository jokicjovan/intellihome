import React, { useState } from 'react';
import SmartDeviceRegistrationForm from "../Shared/SmartDeviceRegistrationForm.tsx";
import {Box, Button, Container, Typography} from "@mui/material";
import CommonSmartDeviceFields from "../Shared/CommonSmartDeviceFields.ts";
import axios from "axios";
import {environment} from "../../../../security/Environment.tsx";

interface AmbientSensorRegistrationFormProps {
    smartHomeId: string;
}

const AmbientSensorRegistrationForm : React.FC<AmbientSensorRegistrationFormProps> = ({smartHomeId}) => {
    const [commonFormData, setCommonFormData] = useState<CommonSmartDeviceFields>({
        PowerPerHour: 0,
        Name: "Ambient Sensor",
        Image: new Blob([])
    });

    const handleCommonFormInputChange = (smartDeviceData: CommonSmartDeviceFields) => {
        setCommonFormData(smartDeviceData);
    };
    const handleAirConditionerSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        axios.post(
            `${environment}/api/PKA/CreateAmbientSensor`,
            {...commonFormData},
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
                    Add Ambient Sensor
                </Typography>

                <SmartDeviceRegistrationForm
                    formData={commonFormData}
                    onFormChange={handleCommonFormInputChange}
                />

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

export default AmbientSensorRegistrationForm;