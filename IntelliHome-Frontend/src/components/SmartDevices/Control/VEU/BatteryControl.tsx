import {
    Box,
    Container,
    CssBaseline,
    FormControlLabel,
    IconButton,
    styled,
    Switch,
    SwitchProps,
    Typography
} from "@mui/material";
import React, {useState} from "react";
import {LineChart} from "@mui/x-charts";
import {Chart} from "react-google-charts";

const BatteryControl = () => {
    const [capacity, setCapacity] = useState(800)
    const [currentCapacity, setCurrentCapacity] = useState(85)
    const [usageLastMinute, setUsageLastMinute] = useState(85)
    const [data1, setData1] = useState([["date", "dogs", "cats"],
        [new Date().toDateString(), 1, 2],
        [new Date().toDateString(), 2, 7]])

    const options1 = {
        hAxis: {
            title: "XLABEL",
        },
        vAxis: {
            title: "YLABEL",
        }
    };

    return <><Box mt={1} display="grid" gap="10px" gridTemplateColumns="4fr 3fr 1fr"
                  gridTemplateRows="170px 170px 170px">

        <Box gridColumn={2} height="170px" gridRow={1} display="flex" justifyContent="center" flexDirection="column"
             alignItems="center" bgcolor="white" borderRadius="25px">
            <Typography fontSize="20px" fontWeight="600"> Capacity</Typography>
            <Typography fontSize="50px" fontWeight="700">{capacity}kW</Typography>

        </Box>
        <Box gridColumn={2} height="170px" gridRow={2} display="flex" justifyContent="center" flexDirection="column"
             alignItems="center" bgcolor="white" borderRadius="25px">
            <Typography fontSize="20px" fontWeight="600"> Current Capacity</Typography>
            <Typography fontSize="50px" fontWeight="700">{currentCapacity}kW</Typography>

        </Box>
        <Box gridColumn={1} height="350px" gridRow={1} display="flex" justifyContent="center" flexDirection="column"
             alignItems="center" bgcolor="white" borderRadius="25px">
            <Chart
                chartType="LineChart"
                width="100%"
                height="300px"
                data={data1}
                options={options1}
            />
        </Box>

    </Box></>
}

export default BatteryControl;