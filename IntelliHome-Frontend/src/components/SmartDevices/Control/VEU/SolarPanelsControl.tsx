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
import {KeyboardArrowDown, KeyboardArrowUp} from "@mui/icons-material";
import {LineChart} from "@mui/x-charts";

const SolarPanelsControl = () => {
    const [area,setArea]=useState(800)
    const [efficiency,setEfficiency]=useState(85)
    const [isOn,setIsOn]=useState(false)
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
                  gridTemplateRows="170px 170px 170px 170px">

        <Box gridColumn={1} height="350px" gridRow={1} display="flex" justifyContent="center" flexDirection="column"
             alignItems="center" bgcolor="white" borderRadius="25px">
            <Typography fontSize="50px" fontWeight="600"> POWER</Typography>
            <FormControlLabel sx={{marginRight: 0}}
                              control={<SwitchPower sx={{ml: "10px", mt: "20px"}}/>}
            />
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

        </Box><Box gridColumn={3} height="350px" gridRow={1} display="flex" justifyContent="center" flexDirection="column"
                   alignItems="center" bgcolor="white" borderRadius="25px">
        <LineChart
            xAxis={[{ data: [1, 2, 3, 5, 8, 10] }]}
            series={[
                {
                    data: [2, 5.5, 2, 8.5, 1.5, 5],
                    area: true,
                    showMark:false,
                    color:"rgb(52,63,113)"

                },
            ]}
            sx={{
                '& .MuiAreaElement-series-Germany': {
                    fill: "rgba(52,63,113,0.3)",
                },
            }}
            width="600"
            height="300"
        />

    </Box>
    </Box></>
}

export default SolarPanelsControl;