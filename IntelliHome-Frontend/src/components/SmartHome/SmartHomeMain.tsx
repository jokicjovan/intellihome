import React, {useEffect, useState} from "react";
import axios from "axios";
import {environment} from "../../utils/Environment.ts";
import {
    Box,
    Button,
    Container,
    Dialog,
    Grid,
    Menu,
    MenuItem,
    TablePagination,
    Typography
} from "@mui/material";
import {Add, LocationOn, ShowChart} from "@mui/icons-material";
import SmartDeviceCard from "../SmartDevices/SmartDeviceCard.tsx";
import AirConditionerRegistrationForm from "../SmartDevices/Registration/PKA/AirConditionerRegistrationForm.tsx";
import AmbientSensorRegistrationForm from "../SmartDevices/Registration/PKA/AmbientSensorRegistrationForm.tsx";
import WashingMachineRegistrationForm from "../SmartDevices/Registration/PKA/WashingMachineRegistrationForm.tsx";
import LampRegistrationForm from "../SmartDevices/Registration/SPU/LampRegistrationForm.tsx";
import SprinklerRegistrationForm from "../SmartDevices/Registration/SPU/SprinklerRegistrationForm.tsx";
import VehicleChargerRegistrationForm from "../SmartDevices/Registration/VEU/VehicleChargerRegistrationForm.tsx";
import VehicleGateRegistrationForm from "../SmartDevices/Registration/SPU/VehicleGateRegistrationForm.tsx";
import BatterySystemRegistrationForm from "../SmartDevices/Registration/VEU/BatterySystemRegistrationForm.tsx";
import SolarPanelSystemRegistrationForm from "../SmartDevices/Registration/VEU/SolarPanelSystemRegistrationForm.tsx";
import SignalRSmartHomeService from "../../services/smartDevices/SignalRSmartHomeService.ts";
import SmartHomeReport from "./SmartHomeReport.tsx";


const SmartHomeMain = ({smartHomeId}) => {
    const [smartHome, setSmartHome] = useState({
        name: "",
        address: "",
        type: 0,
        area: 0,
        city: {name: "", country: ""},
        numberOfFloors: 0
    });
    const [page, setPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(8);
    const [totalCount, setTotalCount] = useState(0);
    const textStyle = {fontStyle: "bold", fontWeight: "600", color: "#343F71", margin: "5px"};
    const iconStyle = {fontStyle: "bold", fontWeight: "600", color: "#343F71", margin: "5px"};
    const buttonStyle = {
        backgroundColor: "#FBC40E",
        boxShadow: "0 2px 5px rgba(0, 0, 0, 0.2)",
        width: "200px",
        height: "40px",
        fontSize: "20px",
        fontWeight: "600",
        margin: "15px",
        borderRadius: "5px",
        ':hover': {backgroundColor: "#EDB90D"},
        textTransform: "none"
    }
    const statsButtonStyle = {
        backgroundColor: "#343F71",
        boxShadow: "0 2px 5px rgba(0, 0, 0, 0.2)",
        width: "100px",
        height: "40px",
        fontSize: "20px",
        fontWeight: "600",
        margin: "15px",
        borderRadius: "5px",
        ':hover': {backgroundColor: "#20284b"},
        textTransform: "none"
    }
    const typoStyle = {color: "white", fontWeight: "600", fontSize: "15px"}
    const [smartDevices, setSmartDevices] = useState([]);
    const [anchorEl, setAnchorEl] = useState(null);
    const [openModal, setOpenModal] = useState(false);
    const [modalContentItem, setModalContentItem] = useState(-1);
    const signalRSmartHomeService = new SignalRSmartHomeService();

    const subscriptionResultCallback = (result) => {
        console.log('Subscription result:', result);
    };

    useEffect(() => {
        if (smartHomeId) {
            getSmartHome();
        }

        signalRSmartHomeService.startConnection().then(() => {
                console.log('SignalR connection established');
                signalRSmartHomeService.receiveSmartHomeSubscriptionResult(subscriptionResultCallback);
                signalRSmartHomeService.subscribeToSmartHome(smartHomeId)
            }
        );

        return () => {
            signalRSmartHomeService.stopConnection().then(() => {
                console.log('SignalR connection stopped...');
            });
        }
    }, [smartHomeId]);

    const getSmartHome = () => {
        axios.get(environment + `/api/SmartHome/GetSmartHome?Id=${smartHomeId}`).then(res => {
            setSmartHome(res.data);
        }).catch(err => {
            console.log(err)
        });
    }

    const renderPanel = () => {
        return <>
            {smartDevices.length === 0 ? <p>No smart devices to show...</p> : <div>
                <Grid container sx={{boxSizing: 'border-box', mt: 1, height: '100%', width: '100%', px: 3}}>
                    {smartDevices.map((item) => (
                        <Grid item key={item.id} xs={12} sm={6} md={6} lg={6}>
                            <SmartDeviceCard smartDevice={item}/>
                        </Grid>
                    ))}
                </Grid>
            </div>
            }
        </>
    }

    useEffect(() => {
        getSmartDevices()
    }, [page, rowsPerPage])

    const getSmartDevices = () => {
        axios.get(environment + `/api/SmartDevice/GetSmartDevicesForHome/${smartHomeId}?PageNumber=${page + 1}&PageSize=${rowsPerPage}`).then(res => {
            setSmartDevices(res.data.smartDevices);
            setTotalCount(res.data.totalCount);
        }).catch(err => {
            console.log(err)
        });
    }

    const handleChangePage = (event, newPage) => {
        setPage(newPage);
    };

    const handleClick = (event) => {
        setAnchorEl(event.currentTarget);
    };

    const handleStatsClick = (event) => {
        setAnchorEl(null);
        handleOpenModal(99)
    };

    const handleCloseModal = () => {
        setOpenModal(false);
        getSmartDevices();
    };

    const handleOpenModal = (item) => {
        setModalContentItem(item);
        setOpenModal(true);
    }

    const modalContent = (
        <Box sx={{
            width: "100%",
            height: "100%",
            backgroundColor: "white",
            display: "flex",
            flexDirection: "column",
            position: "relative"
        }}>
            {modalContentItem === 0 &&
                <AirConditionerRegistrationForm smartHomeId={smartHomeId} onClose={handleCloseModal}/>}
            {modalContentItem === 1 &&
                <AmbientSensorRegistrationForm smartHomeId={smartHomeId} onClose={handleCloseModal}/>}
            {modalContentItem === 2 &&
                <WashingMachineRegistrationForm smartHomeId={smartHomeId} onClose={handleCloseModal}/>}
            {modalContentItem === 3 && <LampRegistrationForm smartHomeId={smartHomeId} onClose={handleCloseModal}/>}
            {modalContentItem === 4 &&
                <SprinklerRegistrationForm smartHomeId={smartHomeId} onClose={handleCloseModal}/>}
            {modalContentItem === 5 &&
                <VehicleGateRegistrationForm smartHomeId={smartHomeId} onClose={handleCloseModal}/>}
            {modalContentItem === 6 &&
                <BatterySystemRegistrationForm smartHomeId={smartHomeId} onClose={handleCloseModal}/>}
            {modalContentItem === 7 &&
                <SolarPanelSystemRegistrationForm smartHomeId={smartHomeId} onClose={handleCloseModal}/>}
            {modalContentItem === 8 &&
                <VehicleChargerRegistrationForm smartHomeId={smartHomeId} onClose={handleCloseModal}/>}

            {modalContentItem === 99 &&
                <SmartHomeReport smartHomeId={smartHomeId}/>}

        </Box>
    );
    return (
        <Box sx={{width: "100%", height: "100%", backgroundColor: "#DBDDEB"}}>
            <Container maxWidth="xl" sx={{
                display: "flex",
                flexDirection: "row",
                justifyContent: "space-between",
                alignItems: "center",
                mt: "10px",
            }}>
                <Container maxWidth="xl" sx={{display: "flex", flexDirection: "column"}}>
                    <Typography mb={1} align="left"
                                sx={{fontSize: "40px", fontWeight: "600"}}>{smartHome.name}</Typography>
                    <Container disableGutters sx={{display: "flex", flexDirection: "row"}}>
                        <LocationOn sx={iconStyle}/><Typography
                        sx={textStyle}>{smartHome.address}, {smartHome.city.name}, {smartHome.city.country}</Typography>
                    </Container>
                </Container>

                <Container sx={{display: "flex", flexDirection: "row", height: "70px"}}>
                    <Container sx={{display: "flex", flexDirection: "column", alignItems: "center"}}>
                        <Typography sx={{fontSize: "18px", fontWeight: "600"}}>Type</Typography>
                        <Typography sx={{
                            fontSize: "25px",
                            fontWeight: "600",
                            color: "#343F71",
                            mt: "5px"
                        }}>{smartHome.type == 0 ? "House" : "Flat"}</Typography>
                    </Container>
                    <div style={{height: "100%", borderRight: "2px solid #FBC40E", margin: "0 10px"}}></div>
                    <Container sx={{display: "flex", flexDirection: "column", alignItems: "center"}}>
                        <Typography sx={{fontSize: "18px", fontWeight: "600"}}>Area</Typography>
                        <Typography sx={{
                            fontSize: "25px",
                            fontWeight: "600",
                            color: "#343F71"
                        }}>{smartHome.area}m<sup>2</sup></Typography>
                    </Container>
                    <div style={{height: "100%", borderRight: "2px solid #FBC40E", margin: "0 10px"}}></div>
                    <Container sx={{display: "flex", flexDirection: "column", alignItems: "center"}}>
                        <Typography sx={{fontSize: "18px", fontWeight: "600"}}>Floor</Typography>
                        <Typography sx={{
                            fontSize: "25px",
                            fontWeight: "600",
                            color: "#343F71",
                            mt: "5px"
                        }}>{smartHome.numberOfFloors}</Typography>
                    </Container>
                </Container>

            </Container>


            <Box sx={{
                width: "98%",
                height: "3px",
                borderRadius: "15px",
                background: `#343F71`,
                margin: "10px auto"
            }}></Box>

            <Container maxWidth="xl" sx={{
                display: "flex",
                flexDirection: "row",
                justifyContent: "space-between",
                alignItems: "center",
                mt: "10px"
            }}>
                <Box sx={{
                    background: "white",
                    borderRadius: "5px",
                    padding: "2px 5px",
                    display: "flex",
                    flexDirection: "row",
                    height: "20px"
                }}>
                    <Typography sx={{fontSize: "15px", fontWeight: "600"}}>All devices</Typography>
                    <div style={{height: "100%", borderRight: "2px solid gray", margin: "0 10px"}}></div>
                    <div style={{
                        height: "60%",
                        borderRight: "4px solid #676E79",
                        borderRadius: "2px",
                        margin: "auto 5px"
                    }}></div>
                    <Typography sx={{fontSize: "15px", fontWeight: "600"}}>Indoor home devices</Typography>
                    <div style={{height: "100%", borderRight: "2px solid gray", margin: "0 10px"}}></div>
                    <div style={{
                        height: "60%",
                        borderRight: "4px solid #F43F5E",
                        borderRadius: "2px",
                        margin: "auto 5px"
                    }}></div>
                    <Typography sx={{fontSize: "15px", fontWeight: "600"}}>Outdoor home devices</Typography>
                    <div style={{height: "100%", borderRight: "2px solid gray", margin: "0 10px"}}></div>
                    <div style={{
                        height: "60%",
                        borderRight: "4px solid #2691D9",
                        borderRadius: "2px",
                        margin: "auto 5px"
                    }}></div>
                    <Typography sx={{fontSize: "15px", fontWeight: "600"}}>High power devices</Typography>

                </Box>
                <Button onClick={handleStatsClick} sx={statsButtonStyle}><ShowChart sx={{marginX: "5px", color: "white"}}
                                                                    fontSize="inherit"/><Typography sx={typoStyle}>Stats</Typography></Button>
                <Button onClick={handleClick} sx={buttonStyle}><Add sx={{marginX: "5px", color: "white"}}
                                                                    fontSize="inherit"/><Typography sx={typoStyle}>Add
                    new device</Typography></Button>
                <Menu open={Boolean(anchorEl)} onClose={() => setAnchorEl(null)} anchorEl={anchorEl}
                      anchorOrigin={{vertical: 'bottom', horizontal: 'center'}}
                      transformOrigin={{vertical: 'top', horizontal: 'center'}}>
                    <MenuItem onClick={() => {
                        setAnchorEl(null);
                        handleOpenModal(0)
                    }}>Air conditioner</MenuItem>
                    <MenuItem onClick={() => {
                        setAnchorEl(null);
                        handleOpenModal(1)
                    }}>Ambient sensor</MenuItem>
                    <MenuItem onClick={() => {
                        setAnchorEl(null);
                        handleOpenModal(2)
                    }}>Washing machine</MenuItem>
                    <MenuItem onClick={() => {
                        setAnchorEl(null);
                        handleOpenModal(3)
                    }}>Lamp</MenuItem>
                    <MenuItem onClick={() => {
                        setAnchorEl(null);
                        handleOpenModal(4)
                    }}>Sprinkler</MenuItem>
                    <MenuItem onClick={() => {
                        setAnchorEl(null);
                        handleOpenModal(5)
                    }}>Vehicle gate</MenuItem>
                    <MenuItem onClick={() => {
                        setAnchorEl(null);
                        handleOpenModal(6)
                    }}>Battery system</MenuItem>
                    <MenuItem onClick={() => {
                        setAnchorEl(null);
                        handleOpenModal(7)
                    }}>Solar system</MenuItem>
                    <MenuItem onClick={() => {
                        setAnchorEl(null);
                        handleOpenModal(8)
                    }}>Vehicle charger</MenuItem>
                </Menu>
            </Container>

            <Container style={{position: 'relative'}}>
                {renderPanel()}
                <TablePagination
                    component="div"
                    count={totalCount}
                    page={page}
                    onPageChange={handleChangePage}
                    rowsPerPage={rowsPerPage}
                    rowsPerPageOptions={[8]}
                    style={{position: 'fixed', bottom: 10, right: 30}}
                />
            </Container>

            <Dialog open={openModal} onClose={handleCloseModal}>
                {modalContent}
            </Dialog>

        </Box>
    )

}

export default SmartHomeMain;