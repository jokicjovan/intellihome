import React, { useState } from 'react';
import SmartDeviceRegistrationForm from "../Shared/SmartDeviceRegistrationForm.tsx";
import {Box, Container, TextField, Typography} from "@mui/material";
import CommonSmartDeviceFields from "../../../../models/interfaces/CommonSmartDeviceFields.ts";
import InputAdornment from "@mui/material/InputAdornment";
import SmartDeviceService from "../../../../services/smartDevices/SmartDeviceService.ts";
import smartDeviceCategory from "../../../../models/enums/SmartDeviceCategory.ts";
import SmartDeviceType from "../../../../models/enums/SmartDeviceType.ts";
import DeviceRegistrationButtons from "../Shared/DeviceRegistrationButtons.tsx";
import {useMutation, useQueryClient} from "react-query";
import SmartDeviceCategory from "../../../../models/enums/SmartDeviceCategory.ts";
import {RotatingLines} from "react-loader-spinner";

interface VehicleChargerAdditionalFields {
    PowerPerHour: number;
    NumberOfChargingPoints: number;
}

interface VehicleChargerRegistrationFormProps {
    smartHomeId: string;
    onClose: () => void;
}

const VehicleChargerRegistrationForm : React.FC<VehicleChargerRegistrationFormProps> = ({smartHomeId, onClose}) => {
    const queryClient = useQueryClient();
    const [isLoading, setIsLoading] = useState(false);
    const smartDeviceService = new SmartDeviceService();
    const [additionalFormData, setAdditionalFormData] = useState<VehicleChargerAdditionalFields>({
        PowerPerHour: 1,
        NumberOfChargingPoints: 2
    });

    const [commonFormData, setCommonFormData] = useState<CommonSmartDeviceFields>({
        Name: "VehicleCharger",
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

    interface registrationMutationInterface {
        formData: any;
        smartHomeId: string;
        deviceCategory: SmartDeviceCategory;
        deviceType: SmartDeviceType;
    }

    const registrationMutation = useMutation(
        (params: registrationMutationInterface) => {
            const { formData, smartHomeId, deviceCategory, deviceType } = params;
            setIsLoading(true);
            return smartDeviceService.registerSmartDevice(formData, smartHomeId, deviceCategory, deviceType)
        },
        {
            onSuccess: (res) => {
                setIsLoading(false);
                if (res.status === 200) {
                    queryClient.invalidateQueries('smartDevicesForHome');
                    onClose();
                }
            },
            onError: (error) => {
                console.error('Error:', error);
            },
        }
    );

    const handleVehicleChargerSubmit = (e: React.FormEvent) => {
        e.preventDefault();

        const formData : any = {...commonFormData, ...additionalFormData}
        const payload = {formData : formData , smartHomeId : smartHomeId, deviceCategory : SmartDeviceCategory.VEU, deviceType: SmartDeviceType.VEHICLECHARGER}
        registrationMutation.mutate(payload);
    };

    return (<Box>{isLoading &&
        <Box sx={{position:"fixed", top:0, left:0, width:"100%", height:"100%", display:"flex", justifyContent:"center", alignItems:"center", zIndex:"9999", backgroundColor:"rgba(0,0,0,0.7)"}}>
            <RotatingLines
                visible={true}
                height="96"
                width="96"
                color="grey"
                strokeWidth="5"
                animationDuration="0.75"
                ariaLabel="rotating-lines-loading"
                strokeColor={"#FBC40E"}
                wrapperStyle={{}}
                wrapperClass=""/>
        </Box>}
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
                onSubmit={handleVehicleChargerSubmit}
                sx={{
                    display: "flex",
                    flexDirection: "column",
                    alignItems: "start",
                    width: 1,
                    margin: 2
                }}
            >
                <Typography variant="h4" sx={{ textAlign: "left", width: 1 }}>
                    Add Vehile Charger
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
                        id="PowerPerHour"
                        label="PowerPerHour"
                        name="PowerPerHour"
                        type="number"
                        value={additionalFormData.PowerPerHour}
                        onChange={handleAdditionalFormInputChange}
                        InputProps={{
                            endAdornment: <InputAdornment position="end">KW</InputAdornment>,
                        }}
                        inputProps={{
                            min: 1,
                            max: 1000,
                        }}
                        sx={{marginRight:1}}
                    />

                    <TextField
                        variant="outlined"
                        margin="normal"
                        required
                        fullWidth
                        id="NumberOfChargingPoints"
                        label="Charging points"
                        name="NumberOfChargingPoints"
                        type="number"
                        value={additionalFormData.NumberOfChargingPoints}
                        onChange={handleAdditionalFormInputChange}
                        InputProps={{
                            endAdornment: <InputAdornment position="end">Count</InputAdornment>,
                        }}
                        inputProps={{
                            min: 1,
                            max: 4,
                        }}
                        sx={{marginLeft:1}}
                    />
                </Box>

                <DeviceRegistrationButtons onCancel={onClose} onSubmit={() => {}}/>
            </Box>
        </Container>
    </Box>);
};

export default VehicleChargerRegistrationForm;