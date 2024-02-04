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

interface SolarPanelSystemAdditionalFields {
    Area: number;
    Efficiency: number;
}

interface SolarPanelSystemRegistrationFormProps {
    smartHomeId: string;
    onClose: () => void;
}

const SolarPanelSystemRegistrationForm : React.FC<SolarPanelSystemRegistrationFormProps> = ({smartHomeId, onClose}) => {
    const queryClient = useQueryClient();
    const [isLoading, setIsLoading] = useState(false);
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
                setIsLoading(false);
                console.error('Error:', error);
            },
        }
    );

    const handleSolarPanelSystemSubmit = (e: React.FormEvent) => {
        e.preventDefault();

        const formData : any = {...commonFormData, ...additionalFormData}
        const payload = {formData : formData , smartHomeId : smartHomeId, deviceCategory : SmartDeviceCategory.VEU, deviceType: SmartDeviceType.SOLARPANELSYSTEM}
        registrationMutation.mutate(payload);
    };

    return (<Box>{isLoading &&
        <Box sx={{position:"fixed", top:0, left:0, width:"100%", height:"100%", display:"flex", justifyContent:"center", alignItems:"center", zIndex:"9999", backgroundColor:"rgba(0,0,0,0.7)"}}>
            <RotatingLines
                visible={true}
                width="96"
                strokeWidth="5"
                animationDuration="0.75"
                ariaLabel="rotating-lines-loading"
                strokeColor={"#FBC40E"}/>
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
    </Box>);
};

export default SolarPanelSystemRegistrationForm;