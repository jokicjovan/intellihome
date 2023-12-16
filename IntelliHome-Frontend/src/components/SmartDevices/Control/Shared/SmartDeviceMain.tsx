import {Box, Container, CssBaseline, FormControlLabel, styled, Switch, SwitchProps, Typography} from "@mui/material";
import React, {useEffect, useMemo, useState} from "react";
import AmbientSensorControl from "../PKA/AmbientSensorControl";
import AirConditionerControl from "../PKA/AirConditionerControl";
import LampControl from "../SPU/LampControl";
import SolarPanelsControl from "../VEU/SolarPanelsControl";
import GateControl from "../SPU/GateControl";
import SmartDeviceReportAction from "./SmartDeviceReportAction";
import dayjs from "dayjs";
import BatteryControl from "../VEU/BatteryControl";
import axios from "axios";
import {environment} from "../../../../security/Environment.tsx";
import {useParams} from "react-router-dom";
import SignalRSmartDeviceService from "../../../../services/smartDevices/SignalRSmartDeviceService.ts";
import SmartDevice from "../../../../models/interfaces/SmartDevice.ts";

const SmartDeviceMain = () => {
    const params = useParams();
    const [isConnected, setIsConnected] = useState(false);
    const [selectedTab, setSelectedTab] = useState(0);
    const [deviceType, setDeviceType] = useState(params.type);
    const [smartDeviceId, setSmartDeviceId] = useState(params.id);
    const [isOn, setIsOn] = useState(false);
    // @ts-ignore
    const [smartDevice, setSmartDevice] = useState<SmartDevice>({});

    function getSmartDevice() {
        axios.get(environment + `/api/${deviceType}/Get?Id=${smartDeviceId}`).then(res => {
            setSmartDevice(res.data)
            setIsConnected(res.data.isConnected)
            setIsOn(res.data.isOn)
        }).catch(err => {
            console.log(err)
        });
    }

    function toggleSmartDevice(on) {
        axios.put(environment + `/api/${deviceType}/Toggle?Id=${smartDeviceId}&TurnOn=${on}`).then(res => {
            console.log(res.data)
            setIsOn(on)
            smartDevice.isOn = on;
        }
        ).catch(err => {
            console.log(err)
        });
    }

    const signalRSmartDeviceService = useMemo(() => new SignalRSmartDeviceService(), [smartDeviceId]);
    function initSmartDeviceSocketConnection(){
        const subscriptionResultCallback = (result) => {
            result = JSON.parse(result);
            console.log('Subscription result:', result);
            setSmartDevice(prevSmartDevice => ({
                ...prevSmartDevice,
                ...result
            }));
            result.isConnected !== undefined && setIsConnected(result.isConnected);
            result.isOn !== undefined && setIsOn(result.isOn);
        }

        const resultCallback = (result) => {
            console.log('Subscription result:', result);
        };

        signalRSmartDeviceService.startConnection().then(() => {
            console.log('SignalR connection established');
            console.log(smartDeviceId);
            signalRSmartDeviceService.subscribeToSmartDevice(smartDeviceId);
            signalRSmartDeviceService.receiveSmartDeviceData(subscriptionResultCallback);
            signalRSmartDeviceService.receiveSmartDeviceSubscriptionResult(resultCallback);
        })
    }

    useEffect(() => {
        const fetchData = async () => {
            try {
                if (smartDeviceId) {
                    await getSmartDevice();
                }
                await initSmartDeviceSocketConnection();
            } catch (error) {
                console.error('Error:', error);
            }
        };
        fetchData();

        return () => {
            signalRSmartDeviceService.stopConnection().then(() => {
                console.log('SignalR connection stopped');
            });
        }
    }, [smartDeviceId]);

    const SwitchPower = styled((props: SwitchProps) => (
        <Switch focusVisibleClassName=".Mui-focusVisible" checked={isOn} onChange={(e) => {
            setIsOn(e.target.checked);
            toggleSmartDevice(e.target.checked);
        }} size="medium" disableRipple {...props} />
    ))(({theme}) => ({
        width: 105,
        height: 52,
        padding: 0,
        '& .MuiSwitch-switchBase': {
            padding: 0,
            margin: 4,
            transitionDuration: '300ms',
            '&.Mui-checked': {
                transform: 'translateX(52px)',
                color: '#fff',
                '& + .MuiSwitch-track': {
                    backgroundColor: theme.palette.mode === 'dark' ? '#2ECA45' : '#65C466',
                    opacity: 1,
                    border: 0,
                },
                '&.Mui-disabled + .MuiSwitch-track': {
                    opacity: 0.5,
                },
            },
            '&.Mui-focusVisible .MuiSwitch-thumb': {
                color: '#33cf4d',
                border: '6px solid #fff',
            },
            '&.Mui-disabled .MuiSwitch-thumb': {
                color:
                    theme.palette.mode === 'light'
                        ? theme.palette.grey[100]
                        : theme.palette.grey[600],
            },
            '&.Mui-disabled + .MuiSwitch-track': {
                opacity: theme.palette.mode === 'light' ? 0.7 : 0.3,
            },
        },
        '& .MuiSwitch-thumb': {
            boxSizing: 'border-box',
            width: 44,
            height: 44,
        },
        '& .MuiSwitch-track': {
            borderRadius: 26,
            backgroundColor: theme.palette.mode === 'light' ? '#E9E9EA' : '#39393D',
            opacity: 1,
            transition: theme.transitions.create(['background-color'], {
                duration: 500,
            }),
        },
    }));

    return <>
        <Box display="flex" flexDirection="row" alignItems="center" width="100%">
            <Typography fontSize="40px" fontWeight="650">{smartDevice.name}</Typography>
            <Box mx={1} ml={4} sx={{marginTop: "3px"}} width="15px" height="15px" borderRadius="50px"
                 bgcolor={isConnected ? "green" : "red"}/>
            <Typography fontSize="30px" sx={{marginTop: "3px"}} color={isConnected ? "green" : "red"}
                        fontWeight="500">{isConnected ? "Online" : "Offline"}</Typography>
            <Box display="flex" flexDirection="row" alignItems="center" borderRadius="25px" ml={"auto"}>
                <Typography fontSize="30px" fontWeight="600" mr={"20px"}> POWER </Typography>
                <FormControlLabel sx={{ marginRight: 0 }} control={<SwitchPower/>} />
            </Box>

        </Box>
        <Box width="100%">
            <Typography fontSize="25px" color="secondary" fontWeight="600">{deviceType}</Typography>
        </Box>
        <Box width="100%" height={"4px"} my={2} bgcolor="#343F71FF"></Box>
        <Box display="flex" flexDirection="row">
            <Typography px={2} py={1} sx={{
                backgroundColor: selectedTab == 0 ? "#FBC40E" : "#D0D2E1",
                borderRadius: "12px 0px 0px 12px",
                ':hover': {backgroundColor: selectedTab == 0 ? "#FBC40E" : "#a4a5af", cursor: "pointer"}
            }} onClick={() => setSelectedTab(0)} fontSize="25px" fontWeight="500">Control</Typography>
            <Typography px={2} py={1} sx={{
                backgroundColor: selectedTab == 1 ? "#FBC40E" : "#D0D2E1",
                ':hover': {backgroundColor: selectedTab == 1 ? "#FBC40E" : "#a4a5af", cursor: "pointer"}
            }} fontSize="25px" fontWeight="500" onClick={() => setSelectedTab(1)}>Management</Typography>
            <Typography px={2} py={1} sx={{
                backgroundColor: selectedTab == 2 ? "#FBC40E" : "#D0D2E1",
                borderRadius: "0px 12px 12px 0px",
                ':hover': {backgroundColor: selectedTab == 2 ? "#FBC40E" : "#a4a5af", cursor: "pointer"}
            }} onClick={() => setSelectedTab(2)} fontSize="25px" fontWeight="500">Reports</Typography>
        </Box>
        {selectedTab == 0 ? deviceType == "AmbientSensor" ? <AmbientSensorControl smartDevice={smartDevice}/> :
                deviceType == "AirConditioner" ? <AirConditionerControl smartDevice={smartDevice} setSmartDeviceParent={setSmartDevice}/> :
                    deviceType == "Lamp" ? <LampControl device={smartDevice} setSmartDeviceParent={setSmartDevice}/> :
                        deviceType == "SolarPanelSystem" ? <SolarPanelsControl solarPanelSystem={smartDevice}/> :
                            deviceType == "VehicleGate" ? <GateControl device={smartDevice} setSmartDeviceParent={setSmartDevice}/> :
                                deviceType == "BatterySystem" ? <BatteryControl batterySystem={smartDevice}/> :
                                    <></>


            : selectedTab == 2 ? <SmartDeviceReportAction inputData={[
                {action: "some actino", by: "Vukasin", date: new Date(dayjs().subtract(1, "hour").toString())},
                {action: "some actino", by: "Marko", date: new Date(dayjs().subtract(5, "hour").toString())},
                {action: "some actino", by: "Vukasin", date: new Date(dayjs().subtract(1, "day").toString())},
                {action: "some actino", by: "Vukasin", date: new Date(dayjs().subtract(3, "day").toString())},
                {action: "some actino", by: "Dusan", date: new Date(dayjs().toString())},
                {action: "some actino", by: "Vukasin", date: new Date(dayjs().toString())},
                {action: "some actino", by: "Vukasin", date: new Date(dayjs().toString())},
            ]}/> : <></>}

    </>
}

export default SmartDeviceMain;