import {
    Box, Button, Grid,
    Modal, TextField, Typography
} from "@mui/material";
import React, {useEffect, useState} from "react";
import "./baterry.css"
import {ArrowRightAltRounded, ArrowDownwardRounded, Battery20Rounded, BatteryFullRounded, EvStationRounded, Schedule, StartRounded} from "@mui/icons-material";
import AddIcon from "@mui/icons-material/Add";
import InputAdornment from "@mui/material/InputAdornment";
import axios from "axios";
import {environment} from "../../../../security/Environment.tsx";
import SmartDeviceType from "../../../../models/enums/SmartDeviceType.ts";


const EVChargerControl = ({vehicleCharger}) => {
    const [open, setOpen] = useState(false)
    const [chargerPower, setChargerPower] = useState(0)
    const [maxVehicles, setMaxVehicles] = useState(0)
    const [modalCurrentCapacity, setModalCurrentCapacity] = useState(10)
    const [modalMaxCapacity, setModalMaxCapacity] = useState(200)
    const [modalPercentage, setModalPercentage] = useState(100)
    const [chargingPoints,setChargingPoints]=useState([]) //{initialCapacity:20,endCapacity:90,currentCapacity:40,startTime:"12.12.2022 14:36",endTime:"12.12.2022 15:36", totalConsumption:30}
    const [chargingPointsAvailability, setChargingPointsAvailability] = useState([])

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

    function parseDateString(dateString: string): Date | null {
        const parts = dateString.split(' ');
        const [day, month, year] = parts[0].split('/').map(part => parseInt(part, 10))
        const [hour, minute] = parts[1].split(':').map(part => parseInt(part, 10))
        const parsedDate = new Date(year, month - 1, day, hour, minute);
        return parsedDate;
    }

    const setVehicleChargerData = (vehicleChargerData) => {
        if (vehicleChargerData.busyChargingPoints) {
            setChargingPointsAvailability(prevState => {
                const updatedArray = [...prevState];
                vehicleChargerData.busyChargingPoints.forEach(newElement => {
                    const existingIndex = updatedArray.findIndex(existingElement => {
                        return existingElement.id === newElement.id;
                    });
                    if (existingIndex !== -1) {
                        updatedArray[existingIndex].isFree = false;
                    } else {
                        updatedArray.push(newElement);
                    }
                });
                return updatedArray;
            });
            setChargingPoints(vehicleChargerData.busyChargingPoints);
        }
    }

    const handleVehicleChargerDataChange = async (newVehicleChargerData) => {
        if (Object.keys(newVehicleChargerData).length !== 0) {
            setVehicleChargerData(newVehicleChargerData);
        }
    };

    const handleInitialVehicleChargerData = async (vehicleChargerData) => {
        if (Object.keys(vehicleChargerData).length !== 0) {
            setChargerPower(vehicleChargerData.powerPerHour);
            setMaxVehicles(vehicleChargerData.chargingPoints.length);
            setChargingPointsAvailability(
                vehicleChargerData.chargingPoints.map(({ id, isFree }) => ({ id, isFree }))
            );

            setChargingPoints([...vehicleChargerData.chargingPoints.filter(newElement => !newElement.isFree)]);
        }
    }

    useEffect(() => {
        handleVehicleChargerDataChange(vehicleCharger);
    }, [vehicleCharger]);

    useEffect(() => {
       handleInitialVehicleChargerData(vehicleCharger)
    }, [vehicleCharger.id]);

    const handleVehicleConnection = async (e: React.FormEvent) => {
        e.preventDefault();

        if (vehicleCharger === undefined || Object.keys(vehicleCharger).length === 0) {
            return;
        }

        const freeChargerPointIndex = chargingPointsAvailability.findIndex(obj => obj.isFree === true);
        if (freeChargerPointIndex !== -1){
            const res = await axios.put(
                environment +
                `/api/${SmartDeviceType[vehicleCharger.type]}/ConnectToCharger?vehicleChargerId=${vehicleCharger.id}&vehicleChargingPointId=${chargingPointsAvailability[freeChargerPointIndex].id}`,
            {
                    "initialCapacity": modalCurrentCapacity,
                    "capacity": modalMaxCapacity,
                    "chargeLimit": modalPercentage/100
                 }
            );

            if (res.status == 200){
                axios.get(environment + `/api/${vehicleCharger.type}/Get?Id=${vehicleCharger.id}`).then(newRes => {
                    handleInitialVehicleChargerData(newRes.data);
                })
            }
        }

        setOpen(false)
    }

    const handleVehicleDisconnection = async (chargingPointId) => {
        console.log(chargingPointId)

        const res = await axios.put(
            environment +
            `/api/${SmartDeviceType[vehicleCharger.type]}/DisconnectFromCharger?vehicleChargerId=${vehicleCharger.id}&vehicleChargingPointId=${chargingPointId}`
        );

        if (res.status == 200){
            axios.get(environment + `/api/${vehicleCharger.type}/Get?Id=${vehicleCharger.id}`).then(newRes => {
                handleInitialVehicleChargerData(newRes.data);
            })
        }
    }

    return <Box overflow={"auto"} pr={1} mt={1}>
        <Modal
            open={open}
            onClose={() => setOpen(false)}
            aria-labelledby="modal-modal-title"
            aria-describedby="modal-modal-description"
        >
            <Grid borderRadius="25px"
                  container
                  spacing={2}
                  width="30%"
                  bgcolor="white"
                  component="form"
                  onSubmit={handleVehicleConnection}
                  padding={4}
                  sx={{
                position: "absolute", top: "50%", left: "50%",
                transform: 'translate(-50%, -50%)'}}>
                <Grid item xs={12}>
                    <Typography id="modal-modal-title" variant="h6" component="h2">
                        Connect Vehicle To Charger
                    </Typography>
                </Grid>
                <Grid item xs={12}>
                    <TextField value={modalCurrentCapacity} fullWidth type="number"
                               name="currentCapacity"
                               label="Current Capacity"
                               onChange={(e) => {
                                   setModalCurrentCapacity(e.target.value as unknown as number)
                               }}
                               InputProps={{
                                   endAdornment: <InputAdornment position="end">kWh</InputAdornment>,
                                   inputProps: {min: 1, max: 900},
                               }}
                               placeholder="Current Capacity"
                               sx={styledInput}/>
                </Grid>
                <Grid item xs={12}>
                    <TextField value={modalMaxCapacity} fullWidth type="number"
                               name="maxCapacity"
                               label="Max Capacity"
                               onChange={(e) => {
                                   setModalMaxCapacity(e.target.value as unknown as number)
                               }}
                               InputProps={
                                   {
                                       endAdornment: <InputAdornment position="end">kWh</InputAdornment>,
                                       inputProps: {min: 10, max: 1000},
                                   }
                               }
                               placeholder="Current Capacity"
                               sx={styledInput}/>
                </Grid>
                <Grid item xs={12}>
                    <TextField value={modalPercentage} fullWidth type="number"
                               name="percentage"
                               label="Desired percentage"
                               onChange={(e) => {
                                   setModalPercentage(e.target.value as unknown as number)
                               }}
                               InputProps={
                                   {
                                       endAdornment: <InputAdornment position="end">%</InputAdornment>,
                                       inputProps: {min: 0, max: 100},
                                   }
                               }
                               placeholder="Current Capacity"
                               sx={styledInput}/>
                </Grid>

            <Grid item xs={12} display="flex" alignItems="flex-end" justifyContent="end">
                <Button onClick={() => setOpen(false)} type="submit" sx={{
                    backgroundColor: "transparent",
                    border: "1px solid #EDB90D",
                    color: "black",
                    paddingY: "10px",
                    borderRadius: "7px",
                }}>Cancel</Button>
                <Button type="submit" sx={{
                    backgroundColor: "#FBC40E",
                    color: "black",
                    paddingY: "10px",
                    borderRadius: "7px",
                    ml: "10px",
                    ':hover': {backgroundColor: "#EDB90D"}
                }}>Add</Button>

            </Grid>
            </Grid>
        </Modal>
        <Box>
            <Box mt={1} display="grid" gap="10px" gridTemplateColumns="5fr 5fr"
                 gridTemplateRows="260px">
                <Box gridColumn={1} gridTemplateColumns={2} gridRow={1} display="flex" justifyContent="center"
                     flexDirection="column"
                     alignItems="center" bgcolor="white" borderRadius="25px" height="250px">
                    <Typography fontSize="30px" fontWeight="600"> POWER</Typography>
                    <Box display="flex" flexDirection="row" justifyContent="center"
                         alignItems="center">
                        <Typography fontSize="110px"
                                    fontWeight="700">{chargerPower}kWh</Typography>

                    </Box>
                </Box>
                <Box gridColumn={2} gridRow={1} display="flex" justifyContent="center"
                     flexDirection="column"
                     alignItems="center" bgcolor="white" borderRadius="25px" height="250px">
                    <Typography fontSize="30px" fontWeight="600"> MAX VEHICLES</Typography>
                    <Box display="flex" flexDirection="row" justifyContent="center"
                         alignItems="center">
                        <Typography fontSize="110px"
                                    fontWeight="700">{maxVehicles}</Typography>

                    </Box>
                </Box>
            </Box>
            {vehicleCharger.isOn ? <Grid container>
                {chargingPoints.map((vehicle)=><Grid item mr={2} xs={2.9} height="600px" borderRadius="25px" bgcolor="white" display="flex"
                      flexDirection="column" alignItems="center">
                    <EvStationRounded sx={{
                        fontSize: "100px",
                        textAlign: "center",
                        border: "5px solid greenyellow",
                        color: "greenyellow",
                        marginTop: "20px",
                        borderRadius: "80px"
                    }}/>
                    <Box>
                        <Schedule sx={{fontSize: "30px", textAlign: "center", marginTop: "20px", color: "#6b9b21"}}/>
                        <Box display="flex" flexDirection="row">
                            <Typography sx={{textAlign: "center", marginRight:"5px", fontWeight:"bold"}}>Started:</Typography>
                            <Typography sx={{textAlign: "center"}}>{(new Date(vehicle.startTime)).toUTCString()}</Typography>
                        </Box>
                        <ArrowDownwardRounded sx={{fontSize: "30px"}}/>
                        <Box display="flex" flexDirection="row">
                            <Typography sx={{textAlign: "center", marginRight:"5px", fontWeight:"bold"}}>Finished:</Typography>
                            <Typography sx={{textAlign: "center"}}>{(new Date(vehicle.endTime)).toUTCString()}</Typography>
                        </Box>
                    </Box>
                    <Box display="flex" marginTop="20px" justifyContent="center" alignItems="center" flexDirection="row">
                        <Box>
                            <Box display="flex" flexDirection="column" sx={{alignItems:"center"}}>
                                <Box display="flex" flexDirection="row" sx={{textAlign: "center", alignItems:"center"}}>
                                    <Battery20Rounded sx={{fontSize: "30px", color: "#6b9b21"}}/>
                                    <Typography sx={{textAlign: "center", marginTop: "5px", mr: "10px"}}> {(vehicle.initialCapacity * 100 / vehicle.capacity).toFixed(1)} %</Typography>
                                </Box>
                                <Typography sx={{textAlign: "center", marginTop: "5px", fontSize:"12px" }}> ({(vehicle.initialCapacity).toFixed(1)} KWh)</Typography>
                            </Box>
                        </Box>
                        <ArrowRightAltRounded sx={{fontSize: "30px"}}/>
                        <Box display="flex" flexDirection="column" sx={{alignItems:"center"}}>
                            <Box display="flex" flexDirection="row" sx={{textAlign: "center", alignItems:"center"}}>
                                <BatteryFullRounded sx={{fontSize: "30px", ml: "10px", color: "#6b9b21"}}/>
                                <Typography sx={{marginTop: "5px"}}> {(100 * vehicle.chargeLimit).toFixed(1)} %</Typography>
                            </Box>
                            <Typography sx={{textAlign: "center", marginTop: "5px", marginLeft:"15px", fontSize:"12px" }}> ({(vehicle.capacity).toFixed(1)} KWh)</Typography>
                        </Box>
                    </Box>
                    <Box display="block" marginTop="15px">
                        <div className="battery">
                            <div className="liquid"></div>
                        </div>
                        <Box sx={{display:"flex", flexDirection:"row", marginBottom: "0px", marginLeft:"5px"}}>
                            <h5 color="black" style={{marginBottom:0}}>{(vehicle.currentCapacity * 100 / vehicle.capacity).toFixed(2)}%</h5>
                            <h5 style={{marginBottom:0}}> ({(vehicle.currentCapacity).toFixed(2)} KWh)</h5>
                        </Box>
                    </Box>
                    <Typography sx={{textAlign: "center"}}> {(vehicle.currentCapacity - vehicle.initialCapacity).toFixed(2)} KWh recharged</Typography>
                    <Typography sx={{textAlign: "center", marginTop:"10px", fontWeight:"bold"}}> Status: {vehicle.status}</Typography>
                    <Button style={{
                        backgroundColor: "red",
                        color: "white",
                        padding: "5px 20px",
                        marginTop: "15px"
                    }} onClick={() => handleVehicleDisconnection(vehicle.id)}>Unplug</Button>

                </Grid>)}
                {chargingPoints.length<maxVehicles &&<Grid item mr={2} xs={2.9} height="600px" borderRadius="25px" border="3px dashed #6b9b21" display="flex"
                      flexDirection="column" alignItems="center" justifyContent="center"
                      sx={{':hover': {backgroundColor: "rgba(42,61,13,0.05)", cursor: "pointer"}}}
                      onClick={() => setOpen(true)}>

                    <AddIcon sx={{
                        fontSize: "100px",
                        textAlign: "center",
                        color: "#6b9b21",
                        backgroundColor: "rgba(42,61,13,0.05)",
                        marginTop: "20px",
                        borderRadius: "80px"
                    }}/>
                </Grid>}
            </Grid> :
                <Box marginTop={10} fontSize={30} fontWeight={"bold"} color={"gray"}>*Turn on device to see charging ports*</Box>}
        </Box>
    </Box>
}

export default EVChargerControl;