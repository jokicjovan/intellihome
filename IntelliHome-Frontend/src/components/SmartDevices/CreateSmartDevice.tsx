import { Box, Button, Container, FormControlLabel, Checkbox, TextField, Typography } from "@mui/material";
import React, { useState } from "react";

interface DeviceAttribute {
    name: string;
    label: string;
    type: 'text' | 'number' | 'checkbox';
    checkBoxGroup?: string;
    checkBoxValues?: { name: string; label: string }[];
}

const DynamicForm: React.FC<{
    deviceType: string;
    attributes: DeviceAttribute[];
    onSubmit: (deviceType: string, formData: any) => void;
    }> = ({ deviceType, attributes, onSubmit }) => {
    const initialFormData: Record<string, string | number | boolean | Array<string>> = {};

    attributes.forEach(attribute => {
        if (attribute.type === 'checkbox' && attribute.checkBoxValues) {
            initialFormData[attribute.name] = [];
        } else {
            initialFormData[attribute.name] = '';
        }
    });

    const [formData, setFormData] = useState(initialFormData);

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>, attribute: DeviceAttribute) => {
        const { name, value, type, checked } = e.target;

        if (type === 'checkbox' && attribute.checkBoxValues) {
            setFormData((prevData: any) => {
                const existingGroup = prevData[attribute.name] as Array<string> || [];
                const updatedGroup = checked
                    ? [...existingGroup, value]
                    : existingGroup.filter((checkboxValue) => checkboxValue !== value);

                return {
                    ...prevData,
                    [attribute.name]: updatedGroup.length ? updatedGroup : undefined,
                };
            });
        } else {
            setFormData((prevData) => ({
                ...prevData,
                [name]: value,
            }));
        }
    };

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        onSubmit(deviceType, formData);
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
                onSubmit={handleSubmit}
                sx={{
                    display: "flex",
                    flexDirection: "column",
                    alignItems: "start",
                    width: 1,
                    margin: 2
                }}
            >
                <Typography variant="h4" sx={{ textAlign: "left", width: 1 }}>
                    Add {deviceType}
                </Typography>

                {attributes.map((attribute) => (
                    <div key={attribute.name}>
                        {attribute.type === 'checkbox' && attribute.checkBoxValues ? (
                            attribute.checkBoxValues.map((option) => (
                                <FormControlLabel
                                    key={option.name}
                                    control={
                                        <Checkbox
                                            checked={Array.isArray(formData[attribute.name]) && formData[attribute.name]?.includes(option.name)}
                                            onChange={(e) => handleInputChange(e, attribute)}
                                            name={option.name}
                                            value={option.name}
                                        />
                                    }
                                    label={option.label}
                                />
                            ))
                        ) : (
                            <TextField
                                key={attribute.name}
                                variant="outlined"
                                margin="normal"
                                required
                                id={attribute.name}
                                label={attribute.label}
                                name={attribute.name}
                                type={attribute.type}
                                value={formData[attribute.name]}
                                onChange={(e: any) => handleInputChange(e, attribute)}
                            />
                        )}
                    </div>
                ))}

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

const CreateSmartDevice: React.FC<{ deviceType: string }> = ({ deviceType }) => {
    const deviceTypeAttributes: DeviceAttribute[] = [];

    if (deviceType === "AirConditioner") {
        deviceTypeAttributes.push(
            { name: "power", label: "Power", type: "number" },
            { name: "modes", label: "Modes", type: "checkbox", checkBoxValues: [
                    { name: "ecoMode", label: "Eco" },
                    { name: "fanMode", label: "Fan" },
                    { name: "coolMode", label: "Cool" },
                    { name: "warmMode", label: "Warm" }
                ]}
        );
    }
    else if (deviceType === "AmbientSensor") {
        deviceTypeAttributes.push(
            { name: "power", label: "Power", type: "number" }
        );
    }
    else if (deviceType === "WashingMachine") {
        deviceTypeAttributes.push(
            { name: "power", label: "Power", type: "number" },
            { name: "modes", label: "Modes", type: "checkbox", checkBoxValues: [
                    { name: "20", label: "20°" },
                    { name: "30", label: "30°" },
                    { name: "40", label: "40°" },
                    { name: "60", label: "60°" },
                    { name: "90", label: "90°" }
                ]}
        );
    }
    else if (deviceType === "Lamp") {
        deviceTypeAttributes.push(
            { name: "power", label: "Power", type: "number" },
            { name: "brightnessLimit", label: "Brightness Limit", type: "number" },
        );
    }
    else if (deviceType === "Sprinkler") {
        deviceTypeAttributes.push(
            { name: "power", label: "Power", type: "number" }
        );
    }
    else if (deviceType === "VehicleGate") {
        deviceTypeAttributes.push(
            { name: "power", label: "Power", type: "number" }
        );
    }

    const handleSubmit = (deviceType: string, formData: any) => {
        console.log(`Form submitted for ${deviceType}:`, formData);
    };

    return <DynamicForm deviceType={deviceType} attributes={deviceTypeAttributes} onSubmit={handleSubmit} />;
};

export default CreateSmartDevice;
