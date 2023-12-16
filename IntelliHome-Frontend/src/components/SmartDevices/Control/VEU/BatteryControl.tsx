import {Box, FormControlLabel, styled, Switch, SwitchProps, Typography} from "@mui/material";
import React, {useEffect, useState} from "react";
import {Chart} from "react-google-charts";
import axios from "axios";
import {environment} from "../../../../security/Environment.tsx";
import SmartDeviceType from "../../../../models/enums/SmartDeviceType.ts";

const BatteryControl = ({batterySystem}) => {
    const [capacity, setCapacity] = useState(batterySystem.capacity)
    const [currentCapacity, setCurrentCapacity] = useState(batterySystem.currentCapacity)
    const headerRow = ["Date", "Current Capacity"];
    const [data, setData] = useState([headerRow])

    const setBatterySystemData = (batterySystemData) => {
        setCapacity(batterySystemData.capacity);
        setCurrentCapacity(batterySystemData.currentCapacity);
        setData((prevData) => {
            const filteredData = prevData.filter((_, index) => index !== 1);
            return [...filteredData, [new Date().toUTCString(), currentCapacity]];
        });
    };

    const fetchHistoricalData = async () => {
        if (Object.keys(batterySystem).length === 0) {
            return;
        }

        const startDate = new Date();
        const endDate = new Date(startDate);
        startDate.setHours(endDate.getHours() - 24);

        try {
            const res = await axios.get(
                environment +
                `/api/${SmartDeviceType[batterySystem.type]}/GetCapacityHistoricalData?Id=${batterySystem.id}&from=${startDate.toISOString()}&to=${endDate.toISOString()}`
            );

            const dataRows = res.data.map(({ timestamp, currentCapacity }) => [
                new Date(timestamp).toUTCString(),
                parseFloat(currentCapacity),
            ]);
            const transformedData = [headerRow, ...dataRows];
            setData(transformedData);
        } catch (err) {
            console.log(err);
        }
    };

    const handleBatterySystemDataChange = async (newBatterySystemData) => {
        if (Object.keys(newBatterySystemData).length !== 0) {
            setBatterySystemData(newBatterySystemData);
        }
    };

    useEffect(() => {
        handleBatterySystemDataChange(batterySystem);
    }, [batterySystem]);

    useEffect(() => {
        fetchHistoricalData();
    }, [batterySystem.id]);

    const options = {
        hAxis: {
            title: "Time",
        },
        vAxis: {
            title: "Capacity (KWh)",
        },
        title: "Battery Capacity"
    };

    return <><Box mt={1} display="grid" gap="10px" gridTemplateColumns="4fr 5fr"
                  gridTemplateRows="170px 170px 170px">
        <Box gridColumn={1} height="170px" gridRow={1} display="flex" justifyContent="center" flexDirection="column"
             alignItems="center" bgcolor="white" borderRadius="25px">
            <Typography fontSize="20px" fontWeight="600"> Capacity</Typography>
            <Typography fontSize="50px" fontWeight="700">{capacity}kW</Typography>

        </Box>
        <Box gridColumn={1} height="170px" gridRow={2} display="flex" justifyContent="center" flexDirection="column"
             alignItems="center" bgcolor="white" borderRadius="25px">
            <Typography fontSize="20px" fontWeight="600"> Current Capacity</Typography>
            <Typography fontSize="50px" fontWeight="700">{currentCapacity}kW</Typography>

        </Box>
        <Box gridColumn={2} height="350px" gridRow={1} display="flex" justifyContent="center" flexDirection="column"
             alignItems="center" bgcolor="white" borderRadius="25px">
            <Chart
                chartType="LineChart"
                width="100%"
                height="300px"
                data={data}
                options={options}
            />
        </Box>

    </Box></>
}

export default BatteryControl;