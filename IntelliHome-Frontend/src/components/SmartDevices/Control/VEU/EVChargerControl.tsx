import {
    Box, Button, Grid,
    Modal, TextField, Typography
} from "@mui/material";
import React, {useEffect, useState} from "react";
import "./baterry.css"
import {ArrowRightAltRounded, Battery20Rounded, BatteryFullRounded, EvStationRounded, Schedule} from "@mui/icons-material";
import AddIcon from "@mui/icons-material/Add";
import InputAdornment from "@mui/material/InputAdornment";


const EVChargerControl = ({vehicleCharger}) => {
    const [open, setOpen] = useState(false)
    const [chargerPower, setChargerPower] = useState(150)
    const [maxVehicles, setMaxVehicles] = useState(2)
    const [vehicles,setVehicles]=useState([{startCapacity:20,endCapcity:90,currentCapacity:40,startTime:"12.12.2022 14:36"}])
    const [modalCurrentCapacity, setModalCurrentCapacity] = useState(0)
    const [modalMaxCapacity, setModalMaxCapacity] = useState(0)
    const [modalPercentage, setModalPercentage] = useState(0)
    const [chargingPointsIds, setChargingPointsIds] = useState([])
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
        setChargerPower(vehicleChargerData.powerPerHour);
        setMaxVehicles(vehicleChargerData.chargingPoints.length);
        setChargingPointsIds(vehicleChargerData.chargingPoints.map(obj => obj.id))
        console.log(chargingPointsIds.sort())
        console.log(vehicleChargerData)
    };

    const handleVehicleChargerDataChange = async (newVehicleChargerData) => {
        if (Object.keys(newVehicleChargerData).length !== 0) {
            setVehicleChargerData(newVehicleChargerData);
        }
    };

    useEffect(() => {
        handleVehicleChargerDataChange(vehicleCharger);
    }, [vehicleCharger]);

    return <>
        <Modal
            open={open}
            onClose={() => setOpen(false)}
            aria-labelledby="modal-modal-title"
            aria-describedby="modal-modal-description"
        >
            <Grid borderRadius="25px" container spacing={2} width="30%" bgcolor="white" padding={4} sx={{
                position: "absolute", top: "50%", left: "50%",
                transform: 'translate(-50%, -50%)'
            }}>
                <Grid item xs={12}>
                    <Typography id="modal-modal-title" variant="h6" component="h2">
                        Add Vehicle To Charger
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
                                   endAdornment: <InputAdornment position="end">kWh</InputAdornment>
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
                                       endAdornment: <InputAdornment position="end">kWh</InputAdornment>
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
                <Button type="submit" onClick={() => {
                }} sx={{
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
        <Grid container>
            {vehicles.map((vehicle)=><Grid item mr={2} xs={2.5} height="450px" borderRadius="25px" bgcolor="white" display="flex"
                  flexDirection="column" alignItems="center">
                <EvStationRounded sx={{
                    fontSize: "100px",
                    textAlign: "center",
                    border: "5px solid greenyellow",
                    color: "greenyellow",
                    marginTop: "20px",
                    borderRadius: "80px"
                }}/>
                <Schedule sx={{fontSize: "30px", textAlign: "center", marginTop: "20px", color: "#6b9b21"}}/>
                <Typography sx={{textAlign: "center"}}> {vehicle.startTime}</Typography>
                <Box display="flex" marginTop="20px" justifyContent="center" alignItems="center" flexDirection="row">
                    <Battery20Rounded sx={{fontSize: "30px", color: "#6b9b21"}}/>
                    <Typography sx={{textAlign: "center", marginTop: "5px", mr: "10px"}}> {vehicle.startCapacity}%</Typography>
                    <ArrowRightAltRounded sx={{fontSize: "30px"}}/>
                    <BatteryFullRounded sx={{fontSize: "30px", ml: "10px", color: "#6b9b21"}}/>
                    <Typography sx={{textAlign: "center", marginTop: "5px"}}> {vehicle.endCapcity}%</Typography>
                </Box>
                <Box display="block" marginTop="20px">
                    <div className="battery">
                        <div className="liquid"></div>
                    </div>
                    <h5 color="black" style={{marginBottom: "5px"}}>{vehicle.currentCapacity}%</h5>
                </Box>
                <Button style={{
                    backgroundColor: "red",
                    color: "white",
                    padding: "5px 20px",
                    marginTop: "10px"
                }}>Unplug</Button>

            </Grid>)}
            {vehicles.length<maxVehicles &&<Grid item mr={2} xs={2.5} height="450px" borderRadius="25px" border="3px dashed #6b9b21" display="flex"
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
        </Grid>
    </>
}

export default EVChargerControl;