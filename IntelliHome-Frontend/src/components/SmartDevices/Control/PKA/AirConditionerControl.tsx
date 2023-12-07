import {
    Box, Button, Checkbox, FormControl,
    FormControlLabel,
    Grid,
    IconButton, MenuItem, Modal, Select,
    styled,
    Switch,
    SwitchProps, TextField,
    Typography
} from "@mui/material";
import React, {useState} from "react";
import {Add, Close, KeyboardArrowDown, KeyboardArrowUp} from "@mui/icons-material";
import {LocalizationProvider, StaticDateTimePicker} from "@mui/x-date-pickers";
import {AdapterDayjs} from "@mui/x-date-pickers/AdapterDayjs";
import dayjs, {Dayjs} from "dayjs";
import InputAdornment from "@mui/material/InputAdornment";


const AirConditionerControl = () => {

    const [currentTemperature, setCurrentTemperature] = useState(25)
    const [minTemperature, setMinTemperature] = useState(0)
    const [maxTemperature, setMaxTemperature] = useState(70)
    const [desiredTemperature, setDesiredTemperature] = useState(25)
    const [selectedMode, setSelecteedMod] = useState("AUTO")
    const [isOn, setIsOn] = useState(false)
    const [isThereTimeLimit, setIsThereTimeLimit] = useState(false)
    const [open, setIsOpen] = useState(false)
    type TDate = TDate | null;
    const [value, setValue] = React.useState<TDate>(dayjs());
    const [scheduledMode, setScheduledMode] = useState("AUTO");
    const [scheduled, setScheduled] = useState([
        {date: "11.11.2011.", time: "17.15(60min)", mode: "AUTO"},
        {date: "11.11.2011.", time: "17.15(60min)", mode: "AUTO"},
        {date: "11.11.2011.", time: "17.15(60min)", mode: "AUTO"},
        {date: "11.11.2011.", time: "17.15(60min)", mode: "AUTO"},
        {date: "11.11.2011.", time: "17.15(60min)", mode: "AUTO"},
        {date: "11.11.2011.", time: "17.15(60min)", mode: "AUTO"},
        {date: "11.11.2011.", time: "17.15(60min)", mode: "AUTO"},
        {date: "11.11.2011.", time: "17.15(60min)", mode: "AUTO"},
        {date: "11.11.2011.", time: "17.15(60min)", mode: "AUTO"},
        {date: "11.11.2011.", time: "17.15(60min)", mode: "AUTO"},
        {date: "11.11.2011.", time: "17.15(60min)", mode: "AUTO"},
        {date: "11.11.2011.", time: "17.15(60min)", mode: "AUTO"},
    ]);
    console.log(value)
    const DatePickerStyle = {
        "& .MuiPickersLayout-actionBar": {display: "none"},
        minHeight:"520px"
    }
    const styledInput = {
        "& label.Mui-focused": {
            color: "#FBC40E"
        },
        "& .MuiOutlinedInput-root": {
            "&.Mui-focused fieldset": {
                borderColor: "#FBC40E",
                borderRadius: "10px"

            },
            borderRadius: "10px"
        },
        margin: "8px auto", borderRadius: "10px"

    }
    const SwitchAnalog = styled((props: SwitchProps) => (
        <Switch focusVisibleClassName=".Mui-focusVisible" checked={isOn} onChange={(e) => {
            setIsOn(e.target.checked)
        }} size="large" disableRipple {...props} />
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
        <Modal
            open={open}
            onClose={() => setIsOpen(false)}
            aria-labelledby="modal-modal-title"
            aria-describedby="modal-modal-description"
        >
            <Grid borderRadius="25px" container spacing={2} width="50%" bgcolor="white" padding={4} sx={{
                position: "absolute", top: "50%", left: "50%",
                transform: 'translate(-50%, -50%)'
            }}>
                <Grid item xs={12}>
                    <Typography id="modal-modal-title" variant="h6" component="h2">
                        Add Schedule For Air Conditioner
                    </Typography>
                </Grid>
                <Grid item xs={6}>
                    <LocalizationProvider dateAdapter={AdapterDayjs}>
                        <StaticDateTimePicker sx={DatePickerStyle} disablePast value={value}
                                              onChange={(newDate) => setValue(newDate)}/>
                    </LocalizationProvider>
                </Grid>
                <Grid item xs={6} display="flex" margin="0 auto" justifyContent="center" alignItems="center" flexDirection="column">
                    <Select
                        value={scheduledMode}
                        fullWidth
                        sx={{ borderRadius: "10px"}}
                        onChange={(event) => {
                            setScheduledMode(event.target.value as string)
                        }}

                    >
                        <MenuItem value={"AUTO"}>AUTO</MenuItem>
                        <MenuItem value={"HEAT"}>HEAT</MenuItem>
                        <MenuItem value={"COOL"}>COOL</MenuItem>
                        <MenuItem value={"FAN"}>FAN</MenuItem>
                    </Select>
                    <TextField fullWidth type="number" name="temperatureSchedule"
                               InputProps={{endAdornment: <InputAdornment position="start">°C</InputAdornment>}}
                               placeholder="Desired temperature" sx={styledInput} mb={3}></TextField>
                    <TextField fullWidth disabled={isThereTimeLimit} type="number" name="durationSchedule"
                               InputProps={{endAdornment: <InputAdornment position="start">min</InputAdornment>}}
                               placeholder="Duration" sx={styledInput} mb={3}></TextField>
                    <FormControl sx={{width: "500px"}}>
                        <FormControlLabel
                            label="Without Time Limit"
                            control={
                                <Checkbox
                                    checked={isThereTimeLimit}
                                    onChange={()=>setIsThereTimeLimit(!isThereTimeLimit)}
                                />
                            }
                        />
                    </FormControl>
                </Grid>
                <Grid item xs={12} display="flex" alignItems="flex-end" justifyContent="end">
                    <Button type="submit" sx={{
                        backgroundColor: "#FBC40E",
                        color: "black",
                        paddingY: "10px",
                        borderRadius: "7px",
                        ':hover': {backgroundColor: "#EDB90D"}
                    }}>Cancel</Button>
                    <Button type="submit" sx={{
                        backgroundColor: "#FBC40E",
                        color: "black",
                        paddingY: "10px",
                        borderRadius: "7px",
                        ':hover': {backgroundColor: "#EDB90D"}
                    }}>Create</Button>

                </Grid>
            </Grid>
        </Modal>
        <Box mt={1} display="grid" gap="10px" gridTemplateColumns="4fr 2fr 6fr"
             gridTemplateRows="170px 170px 170px 170px">

            <Box gridColumn={1} height="350px" gridRow={1} display="flex" justifyContent="center" flexDirection="column"
                 alignItems="center" bgcolor="white" borderRadius="25px">
                <Typography fontSize="30px" fontWeight="600"> CURRENT</Typography>
                <Typography fontSize="30px" fontWeight="600"> TEMPERATURE</Typography>
                <Typography fontSize="100px" fontWeight="800">{currentTemperature}°C</Typography>
            </Box>

            <Box gridColumn={2} gridRow={1} display="flex" justifyContent="center" flexDirection="column"
                 alignItems="center" bgcolor="white" borderRadius="25px">
                <Typography fontSize="30px" fontWeight="600"> MIN TEMP</Typography>
                <Typography fontSize="60px" fontWeight="700">{minTemperature}°C</Typography>
            </Box>
            <Box gridColumn={2} gridRow={2} display="flex" justifyContent="center" flexDirection="column"
                 alignItems="center" bgcolor="white" borderRadius="25px">
                <Typography fontSize="30px" fontWeight="600"> MAX TEMP</Typography>
                <Typography fontSize="60px" fontWeight="700">{maxTemperature}°C</Typography>
            </Box>
            <Box gridColumn={1} gridRow={3} display="flex" justifyContent="center" flexDirection="column"
                 alignItems="center" bgcolor="white" borderRadius="25px">
                <Typography fontSize="30px" fontWeight="600"> POWER</Typography>
                <FormControlLabel sx={{marginRight: 0}}
                                  control={<SwitchAnalog sx={{ml: "10px", mt: "20px"}}/>}
                />
            </Box>
            <Box gridColumn={1} gridRow={4} display="flex" justifyContent="center" flexDirection="column"
                 alignItems="center" bgcolor="white" borderRadius="25px">
                <Typography fontSize="30px" fontWeight="600"> DESIRED TEMPERATURE</Typography>
                <Box display="flex" flexDirection="row" justifyContent="center" alignItems="center">
                    <Typography fontSize="60px" fontWeight="700">{desiredTemperature}°C</Typography>
                    <Box display="flex" flexDirection="column">
                        <IconButton
                            onClick={() => setDesiredTemperature(desiredTemperature + 1)}><KeyboardArrowUp/></IconButton>
                        <IconButton
                            onClick={() => setDesiredTemperature(desiredTemperature - 1)}><KeyboardArrowDown/></IconButton>

                    </Box>
                </Box>
            </Box>
            <Box gridColumn={2} gridRow={3} height="350px" display="flex" justifyContent="center" flexDirection="column"
                 alignItems="center" bgcolor="white" borderRadius="25px">
                <Typography fontSize="25px" fontWeight="500">MODE</Typography>
                <Typography my={1.8} fontSize={selectedMode == "AUTO" ? "40px" : "30px"}
                            color={selectedMode == "AUTO" ? "#343F71" : "black"} sx={{cursor: "pointer"}}
                            onClick={() => setSelecteedMod("AUTO")} fontWeight="600">AUTO</Typography>
                <Typography my={1.8} fontSize={selectedMode == "COOL" ? "40px" : "30px"}
                            color={selectedMode == "COOL" ? "#343F71" : "black"} sx={{cursor: "pointer"}}
                            onClick={() => setSelecteedMod("COOL")} fontWeight="600">COOL</Typography>
                <Typography my={1.8} fontSize={selectedMode == "HEAT" ? "40px" : "30px"}
                            color={selectedMode == "HEAT" ? "#343F71" : "black"} sx={{cursor: "pointer"}}
                            onClick={() => setSelecteedMod("HEAT")} fontWeight="600">HEAT</Typography>
                <Typography my={1.8} fontSize={selectedMode == "FAN" ? "40px" : "30px"}
                            color={selectedMode == "FAN" ? "#343F71" : "black"} sx={{cursor: "pointer"}}
                            onClick={() => setSelecteedMod("FAN")} fontWeight="600">FAN</Typography>
            </Box>

            <Box gridColumn={3} gridRow={1} height="710px" display="flex"
                 flexDirection="column"
                 bgcolor="white" borderRadius="25px">
                <Box display="flex" mt={1} justifyContent={"center"}
                     flexDirection="row">
                    <Typography fontSize="30px" fontWeight="600"> SCHEDULED</Typography>
                    <IconButton onClick={() => setIsOpen(true)}
                                sx={{
                                    height: "40px",
                                    width: "40px",
                                    marginTop: "2px",
                                    marginLeft: 2
                                }}><Add/></IconButton>
                </Box>
                <Box display="flex" width="100%" flexDirection="column" overflow="auto">
                    {scheduled.map((item) => <Box>
                        <Box width="98%" margin="0 auto" height={"2px"} bgcolor="rgba(0, 0, 0, 0.20)"/>
                        <Box width={"100%"} my={1} display="flex" alignItems="center" flexDirection={"row"}>
                            <Box display="flex" width="100%" justifyContent="space-between">
                                <Typography ml={2} fontSize="20px" fontWeight="500"> {item.date}</Typography>
                                <Typography fontSize="20px" fontWeight="500"> {item.time}</Typography>
                                <Typography mr={2} fontSize="20px" fontWeight="500"> {item.mode}</Typography>
                            </Box>
                            <IconButton onClick={() => {
                            }}><Close/></IconButton>
                        </Box>
                    </Box>)}
                </Box>
            </Box>


        </Box></>
}

export default AirConditionerControl;