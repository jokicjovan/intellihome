import React, {useEffect, useState} from 'react';
import SmartDeviceRegistrationForm from "../Shared/SmartDeviceRegistrationForm.tsx";
import {Box, Checkbox, Container, FormControlLabel, Typography} from "@mui/material";
import CommonSmartDeviceFields from "../../../../models/interfaces/CommonSmartDeviceFields.ts";
import SmartDeviceService from "../../../../services/smartDevices/SmartDeviceService.ts";
import axios from "axios";
import {environment} from "../../../../utils/Environment.ts";
import SmartDeviceType from "../../../../models/enums/SmartDeviceType.ts";
import smartDeviceCategory from "../../../../models/enums/SmartDeviceCategory.ts";
import PowerPerHourInput from "../Shared/PowerPerHourInput.tsx";
import DeviceRegistrationButtons from "../Shared/DeviceRegistrationButtons.tsx";
import {useMutation, useQueryClient} from "react-query";
import SmartDeviceCategory from "../../../../models/enums/SmartDeviceCategory.ts";
import {RotatingLines} from "react-loader-spinner";

interface WashingMachineAdditionalFields {
    PowerPerHour: number;
    ModesIds: string[];
}

interface WashingMachineRegistrationFormProps {
    smartHomeId: string;
    onClose: () => void;
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


const WashingMachineRegistrationForm : React.FC<WashingMachineRegistrationFormProps> = ({smartHomeId, onClose}) => {
    const queryClient = useQueryClient();
    const [isLoading, setIsLoading] = useState(false);
    const smartDeviceService = new SmartDeviceService();
    const [error, setError] = useState<string | null>(null);

    const [additionalFormData, setAdditionalFormData] = useState<WashingMachineAdditionalFields>({
        ModesIds: [],
        PowerPerHour: 1,
    });

    const [commonFormData, setCommonFormData] = useState<CommonSmartDeviceFields>({
        Name: "Washing Machine",
        Image: new Blob()
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

    const handleCheckboxChange = (mode: string) => {
        const isSelected = additionalFormData.ModesIds.includes(mode);

        setAdditionalFormData((prevData) => ({
            ...prevData,
            ModesIds: isSelected
                ? prevData.ModesIds.filter((selectedMode) => selectedMode !== mode)
                : [...prevData.ModesIds, mode],
        }));
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

    const handleWashingMachineSubmit = (e: React.FormEvent) => {
        e.preventDefault();

        if (additionalFormData.ModesIds.length === 0) {
            setError("At least one mode must be selected!");
            return;
        }
        setError(null);

        const formData : any = {...commonFormData, ...additionalFormData}
        const payload = {formData : formData , smartHomeId : smartHomeId, deviceCategory : SmartDeviceCategory.PKA, deviceType: SmartDeviceType.WASHINGMACHINE}
        registrationMutation.mutate(payload);
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
                onSubmit={handleWashingMachineSubmit}
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

                <PowerPerHourInput
                    onValueChange={handlePowerValueChange}
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
                {error && (
                    <Typography variant="body2" color="error" sx={{ textAlign: "left", width: 1, marginBottom: 1 }}>
                        {error}
                    </Typography>
                )}
                <DeviceRegistrationButtons onCancel={onClose} onSubmit={() => {}}/>
            </Box>
        </Container>
    </Box>);
};

export default WashingMachineRegistrationForm;