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
import React, {useEffect, useState} from "react";
import {Add, Close, KeyboardArrowDown, KeyboardArrowUp} from "@mui/icons-material";
import {LocalizationProvider, StaticDateTimePicker} from "@mui/x-date-pickers";
import {AdapterDayjs} from "@mui/x-date-pickers/AdapterDayjs";
import dayjs, {Dayjs} from "dayjs";
import InputAdornment from "@mui/material/InputAdornment";
import axios from "axios";
import {environment} from "../../../../security/Environment.tsx";


const GateControl = ({device, setSmartDeviceParent}) => {

    const [lastPlate, setLastPlate] = useState(device.currentLicencePlate)
    const [isOpenGate, setIsOpenGate] = useState(device.isOpen)
    const [isPublic, setIsPublic] = useState(device.isPublic)
    const [open, setIsOpen] = useState(false)
    type TDate = TDate | null;
    const [value, setValue] = React.useState<TDate>(dayjs());
    const [history, setHistory] = useState([
        "SM074HZ",
        "SM074HZ",
        "SM074HZ",
        "SM074HZ",
        "SM074HZ",
        "SM074HZ",
        "SM074HZ",
        "SM074HZ",
        "SM074HZ",
        "SM074HZ",
    ]);
    const [myPlates, setMyPlates] = useState(device.allowedLicencePlates ? device.allowedLicencePlates : []);
    // console.log(value)
    const DatePickerStyle = {
        "& .MuiPickersLayout-actionBar": {display: "none"},
        minHeight: "520px"
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
    const SwitchState = styled((props: SwitchProps) => (
        <Switch focusVisibleClassName=".Mui-focusVisible" checked={isOpenGate} onChange={(e) => {
            setIsOpenGate(e.target.checked)
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
    const SwitchMode = styled((props: SwitchProps) => (
        <Switch focusVisibleClassName=".Mui-focusVisible" checked={isPublic} onChange={(e) => {
            setIsPublic(e.target.checked)
        }} size="large" disableRipple {...props} />
    ))(({theme}) => ({
        width: 78,
        height: 41,
        padding: 0,
        '& .MuiSwitch-switchBase': {
            padding: 0,
            margin: 4,
            transitionDuration: '300ms',
            '&.Mui-checked': {
                transform: 'translateX(39px)',
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
            width: 33,
            height: 33,
        },
        '& .MuiSwitch-track': {
            borderRadius: 22,
            backgroundColor: theme.palette.mode === 'light' ? '#E9E9EA' : '#39393D',
            opacity: 1,
            transition: theme.transitions.create(['background-color'], {
                duration: 500,
            }),
        },
    }));

    // useEffect(() => {
    //     axios.put(environment + `/api/VehicleGate/TurnOnSmartDevice?Id=${device.id}&TurnOn=${true}`).then(res => {
    //         console.log(res.data)
    //     }).catch(err => {
    //         console.log(err)
    //     });
    // }, []);

    useEffect(() => {
        device.IsOpen = isOpenGate
        device.IsPublic = isPublic
        device.AllowedLicencePlates = myPlates
        device.CurrentLicencePlate = lastPlate
        setSmartDeviceParent(device)
    }, [isOpenGate, isPublic, myPlates, lastPlate]);


    useEffect(() => {
        console.log(device)
        setIsOpenGate(device.isOpen)
        setIsPublic(device.isPublic)
        setMyPlates(device.allowedLicencePlates)
        setLastPlate(device.licencePlate)
        console.log(device)
    }, [device]);




    return <>
        <Modal
            open={open}
            onClose={() => setIsOpen(false)}
            aria-labelledby="modal-modal-title"
            aria-describedby="modal-modal-description"
        >
            <Grid borderRadius="25px" container spacing={2} width="20%" minWidth="500px" bgcolor="white" padding={4} sx={{
                position: "absolute", top: "50%", left: "50%",
                transform: 'translate(-50%, -50%)'
            }}>
                <Grid item xs={12}>
                    <Typography id="modal-modal-title" variant="h6" component="h2">
                        Add New Plate
                    </Typography>
                </Grid>

                <Grid item xs={12} display="flex" margin="0 auto" justifyContent="center" alignItems="center"
                      flexDirection="column">

                    <TextField fullWidth type="text" name="myPlateModal"
                               placeholder="Plate Number" sx={styledInput} mb={3}></TextField>

                </Grid>
                <Grid item xs={12} display="flex" alignItems="flex-end" justifyContent="end">
                    <Box display="flex">
                        <Button mx={2} type="submit" sx={{
                            backgroundColor: "white",
                            border: "1px solid #FBC40E",
                            color: "black",
                            borderRadius: "7px",
                            marginRight:"20px",
                            ':hover': {backgroundColor: "white"}
                        }}>Cancel</Button>
                        <Button mx={2} type="submit" sx={{
                            backgroundColor: "#FBC40E",
                            border: "1px solid #FBC40E",
                            color: "black",
                            borderRadius: "7px",
                            ':hover': {backgroundColor: "#EDB90D"}
                        }}>Create</Button>
                    </Box>

                </Grid>
            </Grid>
        </Modal>
        <Box mt={1} display="grid" gap="10px" gridTemplateColumns="4fr 7fr"
             gridTemplateRows="170px 170px 200px 140px">

            <Box gridColumn={1} height="350px" gridRow={1} display="flex" flexDirection="column"
                 alignItems="center" bgcolor="white" borderRadius="25px">
                <Typography fontSize="30px" mt={1} fontWeight="600"> STATE</Typography>
                <Typography fontSize="80px" color="#343F71" mt={8}
                            fontWeight="600"> {isOpenGate ? "OPENED" : "CLOSED"}</Typography>
                <FormControlLabel sx={{marginRight: 0}} control={<SwitchState sx={{ml: "10px", mt: "20px"}}/>}
                />
            </Box>
            <Box gridColumn={1} gridRow={3} display="flex" justifyContent="center" flexDirection="column"
                 alignItems="center" bgcolor="white" borderRadius="25px">
                <Typography fontSize="30px" mt={1} mb={2} fontWeight="600"> MODE</Typography>
                <Typography fontSize="40px" color="#343F71"
                            fontWeight="600"> {isPublic ? "PUBLIC" : "PRIVATE"}</Typography>
                <FormControlLabel sx={{marginRight: 0}} control={<SwitchMode sx={{ml: "10px", mt: "5px"}}/>}/>
            </Box>
            <Box gridColumn={1} gridRow={4} display="flex" justifyContent="center" flexDirection="column"
                 alignItems="center" bgcolor="white" borderRadius="25px">
                <Typography fontSize="30px" fontWeight="600"> LAST PLATE</Typography>
                <Typography fontSize="50px" fontWeight="600"> {lastPlate}</Typography>

            </Box>

            <Box gridColumn={2} gridRow={1} height="350px" display="flex"
                 flexDirection="column"
                 bgcolor="white" borderRadius="25px">
                <Box display="flex" mt={1} justifyContent={"center"}
                     flexDirection="row">
                    <Typography fontSize="30px" fontWeight="600"> HISTORY</Typography>

                </Box>
                <Box display="flex" width="100%" flexDirection="column" overflow="auto">
                    {history.map((item) => <Box>
                        <Box width="98%" margin="0 auto" height={"2px"} bgcolor="rgba(0, 0, 0, 0.20)"/>
                        <Box width={"100%"} my={1.5} display="flex" alignItems="center" flexDirection={"row"}>
                            <Box display="flex" width="100%" justifyContent="start">
                                <Typography ml={2} fontSize="20px" fontWeight="500"> {item}</Typography>
                            </Box>
                        </Box>
                    </Box>)}
                </Box>
            </Box>

            <Box gridColumn={2} gridRow={3} height="350px" display="flex"
                 flexDirection="column"
                 bgcolor="white" borderRadius="25px">
                <Box display="flex" mt={1} justifyContent={"center"}
                     flexDirection="row">
                    <Typography fontSize="30px" fontWeight="600"> MY PLATES</Typography>
                    <IconButton onClick={() => setIsOpen(true)}
                                sx={{
                                    height: "40px",
                                    width: "40px",
                                    marginTop: "2px",
                                    marginLeft: 2
                                }}><Add/></IconButton>
                </Box>
                <Box display="flex" width="100%" flexDirection="column" overflow="auto">
                    {myPlates ? myPlates.map((item) => <Box>
                        <Box width="98%" margin="0 auto" height={"2px"} bgcolor="rgba(0, 0, 0, 0.20)"/>
                        <Box width={"100%"} my={1} display="flex" alignItems="center" flexDirection={"row"}>
                            <Box display="flex" width="100%" justifyContent="space-between">
                                <Typography ml={2} fontSize="20px" fontWeight="500"> {item}</Typography>
                            </Box>
                            <IconButton onClick={() => {}}><Close/></IconButton>
                        </Box>
                    </Box>) : <></>}
                </Box>
            </Box>


        </Box></>
}

export default GateControl;