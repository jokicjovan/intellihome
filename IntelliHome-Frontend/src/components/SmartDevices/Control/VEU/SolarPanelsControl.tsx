import {
    Box,
    FormControlLabel,
    styled,
    Switch,
    SwitchProps,
    Typography
} from "@mui/material";
import React, {useEffect, useState} from "react";
import {Chart} from "react-google-charts";
import axios from "axios";
import {environment} from "../../../../security/Environment.tsx";

const SolarPanelsControl = ({solarPanelSystem}) => {
    const [area, setArea] = useState(solarPanelSystem.area)
    const [efficiency, setEfficiency] = useState(solarPanelSystem.efficiency)
    const [isOn, setIsOn] = useState(solarPanelSystem.isOn)
    const [data, setData] = useState([["Date", "Current Production"]])
    const [dataFetched, setDataFetched] = useState(false);

    const setSolarPanelSystemData = (solarPanelSystemData) => {
        setArea(solarPanelSystemData.area);
        setEfficiency(solarPanelSystemData.efficiency);
        setData((prevData) => {
            const filteredData = prevData.filter((_, index) => index !== 1);
            return [...filteredData, [new Date().toUTCString(), solarPanelSystemData.productionPerMinute]];
        });
    };

    const fetchHistoricalData = async () => {
        if (Object.keys(solarPanelSystem).length === 0 || dataFetched) {
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
                `/api/SolarPanelSystem/GetProductionHistoricalData?Id=${solarPanelSystem.id}&from=${startDate.toISOString()}&to=${endDate.toISOString()}`
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
    }, [solarPanelSystem]);

    const options = {
        hAxis: {
            title: "Time",
        },
        vAxis: {
            title: "Production (KWh)",
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
        <Box display="grid" gridColumn={2} height="350px" gridRow={1} gap="10px" gridTemplateRows="170px 170px">
            <Box gridColumn={1} height="170px" gridRow={1} display="flex" justifyContent="center" flexDirection="column"
                 alignItems="center" bgcolor="white" borderRadius="25px">
                <Typography fontSize="30px" fontWeight="600"> Area</Typography>
                <Typography fontSize="50px" fontWeight="700">{area}m<sup>2</sup></Typography>

            </Box>
            <Box gridColumn={1} height="170px" gridRow={2} display="flex" justifyContent="center" flexDirection="column"
                 alignItems="center" bgcolor="white" borderRadius="25px">
                <Typography fontSize="30px" fontWeight="600"> Efficiency</Typography>
                <Typography fontSize="50px" fontWeight="700">{efficiency}%</Typography>

            </Box>

        </Box><Box gridColumn={3} height="350px" gridRow={1} display="flex" justifyContent="center"
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

export default SolarPanelsControl;