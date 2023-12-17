import {
    Box,
    Typography
} from "@mui/material";
import React, {useEffect, useState} from "react";
import {Chart} from "react-google-charts";
import axios from "axios";
import {environment} from "../../../../security/Environment.tsx";
import SmartDeviceType from "../../../../models/enums/SmartDeviceType.ts";

const SolarPanelControl = ({solarPanelSystem}) => {
    const [area, setArea] = useState(solarPanelSystem.area)
    const [efficiency, setEfficiency] = useState(solarPanelSystem.efficiency)
    const [productionPerMinute, setProductionPerMinute] = useState(solarPanelSystem.productionPerMinute)
    const headerRow = ["Date", "KWh per min"];
    const [data, setData] = useState([headerRow])

    const setSolarPanelSystemData = (solarPanelSystemData) => {
        setArea(solarPanelSystemData.area);
        setEfficiency(solarPanelSystemData.efficiency);
        setProductionPerMinute(solarPanelSystemData.productionPerMinute)
        setData((prevData) => {
            const filteredData = prevData.filter((item, index) => {
                    if (index !== 0) {
                        return new Date(item[0]).getHours() + 24 >= new Date().getHours();
                    }
                    return true
                }
            );
            return [...filteredData, [new Date().toUTCString(), solarPanelSystemData.productionPerMinute]];
        });
    };

    const fetchHistoricalData = async () => {
        if (Object.keys(solarPanelSystem).length === 0) {
            return;
        }

        const startDate = new Date();
        const endDate = new Date(startDate);
        startDate.setHours(startDate.getHours() - 24);

        try {
            const res = await axios.get(
                environment +
                `/api/${SmartDeviceType[solarPanelSystem.type]}/GetProductionHistoricalData?Id=${solarPanelSystem.id}&from=${startDate.toISOString()}&to=${endDate.toISOString()}`
            );
            const dataRows = res.data.map(({ timestamp, productionPerMinute }) => [
                new Date(timestamp).toUTCString(),
                parseFloat(productionPerMinute),
            ]);
            const transformedData = [headerRow, ...dataRows];
            setData(transformedData);
        } catch (err) {
            console.log(err);
        }
    };

    const handleSolarPanelSystemDataChange = async (newSolarPanelSystemData) => {
        if (Object.keys(newSolarPanelSystemData).length !== 0) {
            setSolarPanelSystemData(newSolarPanelSystemData);
        }
    };

    useEffect(() => {
        handleSolarPanelSystemDataChange(solarPanelSystem);
    }, [solarPanelSystem]);

    useEffect(() => {
        fetchHistoricalData();
    }, [solarPanelSystem.id]);

    const options = {
        hAxis: {
            title: "Time",
        },
        vAxis: {
            title: "Production (KWh)",
        },
        title: "Recent Power Production Per Minute"
    };

    return <><Box mt={1} display="grid" gap="10px" gridTemplateColumns="4fr 5fr"
                  gridTemplateRows="170px">
        <Box display="grid" gridColumn={1} height="350px" gridRow={1} gap="10px" gridTemplateRows="170px">
            <Box gridColumn={1} height="170px" gridRow={1} display="flex" justifyContent="center" flexDirection="column"
                 alignItems="center" bgcolor="white" borderRadius="25px">
                <Typography fontSize="30px" fontWeight="600"> Area</Typography>
                <Typography fontSize="50px" fontWeight="700">{area}m<sup>2</sup></Typography>
            </Box>
            <Box gridColumn={2} height="170px" gridRow={1} display="flex" justifyContent="center" flexDirection="column"
                 alignItems="center" bgcolor="white" borderRadius="25px">
                <Typography fontSize="30px" fontWeight="600"> Efficiency</Typography>
                <Typography fontSize="50px" fontWeight="700">{efficiency}%</Typography>
            </Box>
        </Box>

        <Box gridColumn={1} height="170px" gridRow={2} display="flex" justifyContent="center" flexDirection="column"
             alignItems="center" bgcolor="white" borderRadius="25px">
            <Typography fontSize="30px" fontWeight="600"> Production per minute</Typography>
            <Typography fontSize="50px" fontWeight="700">{productionPerMinute} KWh/min</Typography>
        </Box>
        <Box gridColumn={2} height="350px" gridRow={1} display="flex" justifyContent="center"
             flexDirection="column"
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

export default SolarPanelControl;