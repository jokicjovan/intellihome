import React, { useState } from 'react';
import SmartDeviceRegistrationForm from "../Shared/SmartDeviceRegistrationForm.tsx";
import {
    Box,
    Button,
    Container,
    Grid,
    IconButton,
    List,
    ListItem,
    ListItemText,
    TextField,
    Typography
} from "@mui/material";
import CommonSmartDeviceFields from "../../../../models/interfaces/CommonSmartDeviceFields.ts";
import SmartDeviceService from "../../../../services/smartDevices/SmartDeviceService.ts";
import SmartDeviceType from "../../../../models/enums/SmartDeviceType.ts";
import smartDeviceCategory from "../../../../models/enums/SmartDeviceCategory.ts";
import PowerPerHourInput from "../Shared/PowerPerHourInput.tsx";
import DeviceRegistrationButtons from "../Shared/DeviceRegistrationButtons.tsx";
import DeleteIcon from '@mui/icons-material/Delete';
import AddIcon from '@mui/icons-material/Add';

interface VehicleGateAdditionalFields {
    AllowedLicencePlates: string[];
    PowerPerHour: number;
}

interface VehicleGateRegistrationFormProps {
    smartHomeId: string;
    onClose: () => void;
}

const VehicleGateRegistrationForm : React.FC<VehicleGateRegistrationFormProps> = ({smartHomeId, onClose}) => {
    const smartDeviceService = new SmartDeviceService();
    const [error, setError] = useState<string | null>(null);

    const [additionalFormData, setAdditionalFormData] = useState<VehicleGateAdditionalFields>({
        PowerPerHour: 1,
        AllowedLicencePlates: []
    });

    const [commonFormData, setCommonFormData] = useState<CommonSmartDeviceFields>({
        Name: "VehicleGate",
        Image: new Blob([])
    });

    const [licensePlate, setLicensePlate] = useState('');
    const [isValidLicensePlate, setIsValidLicensePlate] = useState(true);
    const handleLicencePlateChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const inputValue = event.target.value;
        setLicensePlate(inputValue);

        const patternRegex = /^[A-Z]{2}\d{3,5}[A-Z]{2}$/;
        setIsValidLicensePlate(patternRegex.test(inputValue.trim()));
    };

    const handleAddLicencePlate = () => {
        if (licensePlate.trim() !== '' && isValidLicensePlate) {
            setAdditionalFormData((prevData) => ({
                ...prevData,
                AllowedLicencePlates: [...prevData.AllowedLicencePlates, licensePlate],
            }));
            setLicensePlate('');
            setIsValidLicensePlate(true);
        }
    };

    const handleDeleteLicencePlate = (index: number) => {
        setAdditionalFormData((prevData) => {
            const updatedLicencePlates = [...prevData.AllowedLicencePlates];
            updatedLicencePlates.splice(index, 1);
            return { ...prevData, AllowedLicencePlates: updatedLicencePlates };
        });
    };

    const handlePowerValueChange = (powerValue: number) => {
        setAdditionalFormData((prevData) => ({
            ...prevData,
            PowerPerHour: powerValue
        }));
    };

    const handleCommonFormInputChange = (smartDeviceData: CommonSmartDeviceFields) => {
        setCommonFormData(smartDeviceData);
    };

    const handleVehicleGateSubmit = (e: React.FormEvent) => {
        e.preventDefault();

        if (additionalFormData.AllowedLicencePlates.length === 0) {
            setError("At least one licence plate must be added!");
            return;
        }
        setError(null);

        smartDeviceService.registerSmartDevice({...commonFormData, ...additionalFormData}, smartHomeId, smartDeviceCategory.SPU, SmartDeviceType.VehicleGate)
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
                onSubmit={handleVehicleGateSubmit}
                sx={{
                    display: "flex",
                    flexDirection: "column",
                    alignItems: "start",
                    width: 1,
                    margin: 2
                }}
            >
                <Typography variant="h4" sx={{ textAlign: "left", width: 1 }}>
                    Vehicle Gate
                </Typography>

                <SmartDeviceRegistrationForm
                    formData={commonFormData}
                    onFormChange={handleCommonFormInputChange}
                />

                <PowerPerHourInput
                    onValueChange={handlePowerValueChange}
                />

                <Box sx={{border:1, marginY:1, width:1, borderColor:"lightGray", borderRadius:1}}>
                    <Box sx={{padding:1}}>
                        <Typography variant="h6" gutterBottom>
                            Allowed Licence Plates
                        </Typography>
                        <Grid container spacing={2} alignItems="center">
                            <Grid item>
                                <TextField
                                    label="Add Licence Plate"
                                    variant="outlined"
                                    value={licensePlate}
                                    onChange={handleLicencePlateChange}
                                    error={!isValidLicensePlate}
                                    helperText={!isValidLicensePlate && 'Invalid license plate format'}
                                    placeholder="Example: AB123CD"
                                />
                            </Grid>
                            <Grid item>
                                <Button
                                    variant="contained"
                                    color="secondary"
                                    onClick={handleAddLicencePlate}
                                    startIcon={<AddIcon />}
                                >
                                    Add
                                </Button>
                            </Grid>
                        </Grid>
                        <List
                            sx={{
                                display: 'flex',
                                flexDirection: 'row',
                                flexWrap: 'wrap',
                                gap: 1,
                            }}
                        >
                            {additionalFormData.AllowedLicencePlates.map((plate, index) => (
                                <ListItem
                                    key={index}
                                    sx={{
                                        border: '1px solid #ccc',
                                        borderRadius: '4px',
                                        display: 'flex',
                                        alignItems: 'center',
                                        gap: '8px',
                                        padding: '8px',
                                        width:"180px"
                                    }}
                                >
                                    <ListItemText primary={plate} />
                                    <IconButton
                                        aria-label="delete"
                                        onClick={() => handleDeleteLicencePlate(index)}
                                    >
                                        <DeleteIcon />
                                    </IconButton>
                                </ListItem>
                            ))}
                        </List>
                    </Box>
                </Box>
                {error && (
                    <Typography variant="body2" color="error" sx={{ textAlign: "left", width: 1, marginBottom: 1 }}>
                        {error}
                    </Typography>
                )}

                <DeviceRegistrationButtons onCancel={onClose} onSubmit={() => {}}/>
            </Box>
        </Container>
    );
};

export default VehicleGateRegistrationForm;