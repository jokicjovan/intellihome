import {Box, FormControlLabel, styled, Switch, SwitchProps, Typography} from "@mui/material";
import React, {useEffect, useState} from "react";
import {Chart} from "react-google-charts";
import axios from "axios";
import {environment} from "../../../../security/Environment.tsx";

const BatteryControl = ({batterySystem}) => {
    const [capacity, setCapacity] = useState(100)
    const [currentCapacity, setCurrentCapacity] = useState(0)
    const [isOn,setIsOn]=useState(false)
    const [data, setData] = useState([["Date", "Current Capacity"], [new Date().toUTCString(), 0]])
    const [dataFetched, setDataFetched] = useState(false);
    const setBatterySystemData = (batterySystemData) => {
        setCapacity(batterySystemData.capacity);
        setCurrentCapacity(batterySystemData.currentCapacity);

        setData((prevData) => {
            const filteredData = prevData.filter((_, index) => index !== 1);
            return [...filteredData, [new Date().toUTCString(), currentCapacity]];
        });
    };

    const fetchHistoricalData = async () => {
        if (Object.keys(batterySystem).length === 0 || dataFetched) {
            return;
        }
        setDataFetched(true);

        const startDate = new Date();
        startDate.setUTCHours(0, 0, 0, 0);
        const endDate = new Date();
        endDate.setDate(startDate.getDate() + 1);

        try {
            const res = await axios.get(
                environment +
                `/api/BatterySystem/GetCapacityHistoricalData?Id=${batterySystem.id}&from=${startDate.toISOString()}&to=${endDate.toISOString()}`
            );

            const transformedData = [
                ["Date", "Current Capacity"],
                ...res.data.map(({ timestamp, currentCapacity }) => [
                    new Date(timestamp).toUTCString(),
                    currentCapacity,
                ]),
            ];

            setData(transformedData);
        } catch (err) {
            console.log(err);
        }
    };

    const handleBatterySystemDataChange = async (newBatterySystemData) => {
        console.log("stigao novi");
        if (Object.keys(newBatterySystemData).length !== 0) {
            setBatterySystemData(newBatterySystemData);
        }
    };

    useEffect(() => {
        handleBatterySystemDataChange(batterySystem);
    }, [batterySystem]);

    useEffect(() => {
        fetchHistoricalData();
    }, [batterySystem]);

    const options = {
        hAxis: {
            title: "Time",
        },
        vAxis: {
            title: "Capacity",
        }
    };

    const SwitchPower = styled((props: SwitchProps) => (
        <Switch focusVisibleClassName=".Mui-focusVisible" checked={isOn} onChange={(e) => {
            setIsOn(e.target.checked)
        }} size="large" disableRipple {...props} />
    ))(({theme}) => ({
        width: 210,
        height: 95,
        padding: 0,
        '& .MuiSwitch-switchBase': {
            padding: 0,
            margin: 4,
            transitionDuration: '300ms',
            '&.Mui-checked': {
                transform: 'translateX(114px)',
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
            width: 88,
            height: 88,
        },
        '& .MuiSwitch-track': {
            borderRadius: 52,
            backgroundColor: theme.palette.mode === 'light' ? '#E9E9EA' : '#39393D',
            opacity: 1,
            transition: theme.transitions.create(['background-color'], {
                duration: 500,
            }),
        },
    }));
    return <><Box mt={1} display="grid" gap="10px" gridTemplateColumns="4fr 3fr 5fr"
                  gridTemplateRows="170px 170px 170px">
        <Box gridColumn={1} height="350px" gridRow={1} display="flex" justifyContent="center" flexDirection="column"
             alignItems="center" bgcolor="white" borderRadius="25px">
            <Typography fontSize="50px" fontWeight="600"> POWER</Typography>
            <FormControlLabel sx={{marginRight: 0}}
                              control={<SwitchPower sx={{ml: "10px", mt: "20px"}}/>}
             label=""/>
        </Box>
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
        <Box gridColumn={3} height="350px" gridRow={1} display="flex" justifyContent="center" flexDirection="column"
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