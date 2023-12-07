import {Box, Container, CssBaseline, Typography} from "@mui/material";
import {useState} from "react";
import AmbientSensorControl from "../PKA/AmbientSensorControl";
import AirConditionerControl from "../PKA/AirConditionerControl";
import LampControl from "../SPU/LampControl";
import SolarPanelsControl from "../VEU/SolarPanelsControl";
import GateControl from "../SPU/GateControl";

const SmartDeviceMain = () => {
    const [isConnected, setIsConnected] = useState(false);
    const [selectedTab, setSelectedTab] = useState(0);
    const [deviceType, setDeviceType] = useState("SolarPanel");
    return <>
        <Box display="flex" flexDirection="row" alignItems="center">
            <Typography fontSize="40px" fontWeight="650">Smart Device Name</Typography>
            <Box mx={1} ml={4} sx={{marginTop: "3px"}} width="15px" height="15px" borderRadius="50px"
                 bgcolor={isConnected ? "green" : "red"}/>
            <Typography fontSize="30px" sx={{marginTop: "3px"}} color={isConnected ? "green" : "red"}
                        fontWeight="500">{isConnected ? "Online" : "Offline"}</Typography>
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
        {selectedTab == 0 ? deviceType == "AmbientSensor" ? <AmbientSensorControl/> :
                deviceType == "AirConditioner" ? <AirConditionerControl/> :
                    deviceType == "Lamp" ? <LampControl/> :
                        deviceType == "SolarPanel" ? <SolarPanelsControl/> :
                            deviceType=="Gate"?<GateControl/>:
                                /*deviceType=="Battery"?<BatteryControl/>:
                                    <></>*/
                            <></>


            :/*selectedTab == 2?<SmartDeviceReport/>:*/ <></>}

    </>
}

export default SmartDeviceMain;