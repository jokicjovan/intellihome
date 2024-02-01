import SmartHomesConsumption from "../components/Consumption/SmartHomesConsumption.tsx";
import React, {useState} from "react";
import {Box, Typography} from "@mui/material";
import CitiesConsumption from "../components/Consumption/CitiesConsumption.tsx";

const ConsumptionPage=()=>{
    const [selectedTab, setSelectedTab] = useState(0);
    return <Box
        sx={{display: "flex", flexDirection: "column", width: "100%", backgroundColor: "#DBDDEB", padding: "20px"}}>
        <Typography sx={{fontSize: "45px", fontWeight: "600", marginBottom: "25px"}}>Consumption</Typography>
        <Box width="100%" height={"4px"} my={2} bgcolor="#343F71FF"></Box>
        <Box display="flex" flexDirection="row">
            <Typography px={2} py={1} sx={{
                backgroundColor: selectedTab == 0 ? "#FBC40E" : "#D0D2E1",
                borderRadius: "12px 0px 0px 12px",
                ':hover': {backgroundColor: selectedTab == 0 ? "#FBC40E" : "#a4a5af", cursor: "pointer"}
            }} onClick={() => setSelectedTab(0)} fontSize="25px" fontWeight="500">Smart Homes</Typography>
            <Typography px={2} py={1} sx={{
                backgroundColor: selectedTab == 1 ? "#FBC40E" : "#D0D2E1",
                borderRadius: "0px",
                ':hover': {backgroundColor: selectedTab == 1 ? "#FBC40E" : "#a4a5af", cursor: "pointer"}
            }} onClick={() => setSelectedTab(1)} fontSize="25px" fontWeight="500">Cities</Typography>
        </Box>
        {selectedTab == 0 ? <SmartHomesConsumption></SmartHomesConsumption> : <CitiesConsumption></CitiesConsumption>}
    </Box>
};

export default ConsumptionPage;