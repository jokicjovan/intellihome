import {
    Box, Button, Checkbox, FormControl, FormControlLabel, Grid,
    IconButton, MenuItem, Modal, Select, styled, Switch, SwitchProps, TextField,
    Typography
} from "@mui/material";
import React, {useEffect, useState} from "react";
import {
    Add
} from "@mui/icons-material";
import {LocalizationProvider, StaticDateTimePicker} from "@mui/x-date-pickers";
import {AdapterDayjs} from "@mui/x-date-pickers/AdapterDayjs";
import dayjs from "dayjs";
import InputAdornment from "@mui/material/InputAdornment";
import {v4 as uuidv4} from 'uuid';
import axios from "axios";
import {environment} from "../../../../utils/Environment.ts";


const SprinklerControl = ({smartDevice, setSmartDeviceParent}) => {


    const [isThereTimeLimit, setIsThereTimeLimit] = useState(false)
    const [modalDuration, setModalDuration] = useState(0)
    const [isOn, setIsOn] = useState(false)
    const [isSpraying, setIsSpraying] = useState(smartDevice.isSpraying || false)
    const [open, setIsOpen] = useState(false)
    // @ts-ignore
    type TDate = TDate | null;
    const [value, setValue] = React.useState<TDate>(dayjs());
    const [scheduled, setScheduled] = useState(smartDevice.scheduledTasks || []);
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

    const setSprayingEvent = (isSprayingEvent: boolean) => {
        console.log(smartDevice.id);

        axios.put(environment + `/api/Sprinkler/ToggleSprinklerSpraying?Id=${smartDevice.id}&TurnOn=${isSprayingEvent}`).then(res => {
                setIsSpraying(isSprayingEvent);
                smartDevice.isSpraying = isSprayingEvent;
                setSmartDeviceParent(smartDevice);
            }
        ).catch(err => {
            console.log(err)
        });
    }

    const SwitchPower = styled((props: SwitchProps) => (
        <Switch focusVisibleClassName=".Mui-focusVisible" checked={isSpraying} onChange={(e) => {
            setSprayingEvent(e.target.checked);
        }} size="medium" disableRipple {...props} />
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

    useEffect(() => {
        setIsSpraying(smartDevice.isSpraying || false);
        setScheduled(smartDevice.scheduledTasks || []);
    }, [smartDevice]);

    useEffect(() => {
        smartDevice.scheduledTasks = scheduled;
        smartDevice.isSpraying = isSpraying;
        setSmartDeviceParent(smartDevice);
    }, [scheduled, isSpraying]);

    function parseDateString(dateString: string): Date | null {
        const parts = dateString.split(' ');
        const [day, month, year] = parts[0].split('/').map(part => parseInt(part, 10))
        const [hour, minute] = parts[1].split(':').map(part => parseInt(part, 10))

        const parsedDate = new Date(year, month - 1, day, hour, minute);

        return parsedDate;
    }

    const handleAddSchedule = () => {
        console.log(isThereTimeLimit)
        if (isThereTimeLimit) {
            axios.post(environment + '/api/Sprinkler/AddScheduledWork', {
                id: smartDevice.id,
                isSpraying: true,
                startDate: value.subtract(1, 'hour').format('DD/MM/YYYY HH:mm'),
            })
                .then((res) => {
                    if (res.status == 200) {
                        smartDevice.schedules = [...smartDevice.schedules, {
                            timestamp: value.subtract(1, 'hour').format('DD/MM/YYYY HH:mm').toString(),
                            isSpraying: true,
                        }]
                        setSmartDeviceParent(smartDevice)
                        setModalDuration(30)
                        setValue(dayjs() as TDate)
                        setIsOpen(false)
                    }
                })
                .catch((error) => {
                    console.log(error);
                });
        } else {
            axios.post(environment + '/api/Sprinkler/AddScheduledWork', {
                id: smartDevice.id,
                isSpraying: true,
                startDate: value.subtract(1, 'hour').format('DD/MM/YYYY HH:mm'),
                endDate: value.add(modalDuration, 'minutes').subtract(1, 'hour').format('DD/MM/YYYY HH:mm')
            })
                .then((res) => {
                    if (res.status == 200) {
                        smartDevice.schedules = [...smartDevice.schedules, {
                            timestamp: value.subtract(1, 'hour').format('DD/MM/YYYY HH:mm').toString() + " - " + value.add(modalDuration, 'minutes').subtract(1, 'hour').format('DD/MM/YYYY HH:mm'),
                            isSpraying: true,
                        }]
                        setSmartDeviceParent(smartDevice)
                        setModalDuration(30)
                        setValue(dayjs() as TDate)
                        setIsOpen(false)
                    }
                })
                .catch((error) => {
                    console.log(error);
                });
        }
    }

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
                        Add Schedule For Sprinkler
                    </Typography>
                </Grid>
                <Grid item xs={12}>
                    <LocalizationProvider dateAdapter={AdapterDayjs}>
                        <StaticDateTimePicker sx={DatePickerStyle} ampm={false} disablePast value={value}
                                              onChange={(newDate) => setValue(newDate)}/>
                    </LocalizationProvider>
                </Grid>
                <Grid item xs={6}>
                    <TextField value={modalDuration} fullWidth disabled={isThereTimeLimit} type="number"
                               name="durationSchedule"
                               onChange={(e) => {
                                   setModalDuration(e.target.value as number)
                               }}
                               InputProps={
                                   {
                                       inputProps: {min: 0},
                                       endAdornment: <InputAdornment position="start">min</InputAdornment>
                                   }
                               }
                               placeholder="Duration"
                               sx={styledInput}
                               mb={3}> </TextField>
                </Grid>

                <Grid item xs={6} display="flex" alignItems="center">
                    <FormControl sx={{width: "500px", margin: "0 auto"}}>
                        <FormControlLabel
                            label="Without Time Limit"
                            control={
                                <Checkbox
                                    checked={isThereTimeLimit}
                                    onChange={() => setIsThereTimeLimit(!isThereTimeLimit)}
                                />
                            }
                        />
                    </FormControl>

                </Grid>

                <Grid item xs={12} display="flex" alignItems="flex-end" justifyContent="end">
                    <Button onClick={() => setIsOpen(false)} type="submit" sx={{
                        backgroundColor: "transparent",
                        border: "1px solid #EDB90D",
                        color: "black",
                        paddingY: "10px",
                        borderRadius: "7px",
                    }}>Cancel</Button>
                    <Button type="submit" onClick={() => handleAddSchedule()
                    } sx={{
                        backgroundColor: "#FBC40E",
                        color: "black",
                        paddingY: "10px",
                        borderRadius: "7px",
                        ml: "10px",
                        ':hover': {backgroundColor: "#EDB90D"}
                    }}>Create</Button>

                </Grid>

            </Grid>
        </Modal>
        <Box mt={1} display="grid" gap="10px" gridTemplateColumns="4fr 6fr"
             gridTemplateRows="170px 170px 170px 170px">
            <Box gridColumn={1} height="350px" gridRow={1} display="flex" justifyContent="center" flexDirection="column"
                 alignItems="center" bgcolor="white" borderRadius="25px">
                <Typography fontSize="50px" fontWeight="600"> POWER</Typography>
                <FormControlLabel sx={{marginRight: 0}}
                                  control={<SwitchPower sx={{ml: "10px", mt: "20px"}}/>}
                                  label=""/>
            </Box>

            <Box gridColumn={2} gridRow={1} height="710px" display="flex"
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
                    {scheduled && scheduled.length > 0 && scheduled.sort((a, b) => parseDateString(a.timestamp) - parseDateString(b.timestamp)).map((item) =>
                        <Box key={uuidv4()}>
                            <Box width="98%" margin="0 auto" height={"2px"}
                                 bgcolor="rgba(0, 0, 0, 0.20)"/>
                            <Box width={"100%"} my={1} display="flex" alignItems="center"
                                 flexDirection={"row"}>
                                <Box px={2} display="grid" width="100%"
                                     gridTemplateColumns="6fr 1fr 2fr">
                                    <Typography textAlign="left" gridColumn={1} fontSize="20px"
                                                fontWeight="500"> {item.timestamp}</Typography>
                                    <Typography textAlign="right" gridColumn={3} fontSize="20px"
                                                fontWeight="500"> {item.set_spraying ? "ON" : "OFF"}</Typography>

                                </Box>
                            </Box>
                        </Box>)}
                </Box>
            </Box>


        </Box>
    </>

}

export default SprinklerControl;