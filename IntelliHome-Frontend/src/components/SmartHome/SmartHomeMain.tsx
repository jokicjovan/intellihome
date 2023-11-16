import React, {useEffect, useState} from "react";
import axios from "axios";
import {environment} from "../../security/Environment.tsx";
import {Box, Button, Container, Dialog, Grid, TablePagination, TextField, Typography} from "@mui/material";
import {Add, LocationOn} from "@mui/icons-material";
import SmartDeviceCard from "./SmartDeviceCard.tsx";
import SmartHomeCard from "../User/SmartHomeCard.tsx";


const SmartHomeMain = ( {smartHomeId} ) => {
    const [smartHome, setSmartHome] = useState({name: "", address: "", type: 0, area: 0, city: {name: "", country: ""}, numberOfFloors:0});
    const [page, setPage] = useState(0);
    const [rowsPerPage, setRowsPerPage] = useState(8);
    const [totalCount, setTotalCount] = useState(0);
    const textStyle = { fontStyle: "bold", fontWeight: "600", color: "#343F71", margin: "5px" };
    const iconStyle = { fontStyle: "bold", fontWeight: "600", color: "#343F71", margin: "5px"};
    const buttonStyle={backgroundColor:"#FBC40E", boxShadow: "0 2px 5px rgba(0, 0, 0, 0.2)", width:"200px",  height:"30px", fontSize:"20px",fontWeight:"600",margin:"15px",borderRadius:"5px", ':hover':{backgroundColor:"#EDB90D"}, textTransform: "none"}
    const typoStyle={color:"white", fontWeight:"600", fontSize:"15px"}
    const [smartDevices, setSmartDevices] = useState([]);

    useEffect(() => {
        console.log(smartHomeId)
        if (smartHomeId) {
            getSmartHome();
        }
    }, []);

    const getSmartHome = () => {
        axios.get(environment + `/api/SmartHome/GetSmartHome?Id=${smartHomeId}`).then(res => {
            setSmartHome(res.data);
            console.log(res.data)
        }).catch(err => {
            console.log(err)
        });
    }

    const renderPanel = () => {
        return <>
            {smartDevices.length===0 ? <p>No smart devices to show...</p> : <div>
                <Grid container sx={{ boxSizing: 'border-box', mt: 1, height: '100%', width: '100%', px: 3 }}>
                    {smartDevices.map((item) => (
                        <Grid item key={item.id} xs={12} sm={6} md={4} lg={3}>
                            <SmartDeviceCard smartDevice={item} />
                        </Grid>
                    ))}
                </Grid>
            </div>
            }
        </>
    }

    useEffect( ()=>{
        getSmartDevices()
    },[page, rowsPerPage])

    const getSmartDevices = () => {
        axios.get(environment + `/api/SmartDevice/GetSmartDevices/${smartHomeId}?PageNumber=${page}&PageSize=${rowsPerPage}`).then(res => {
            setSmartDevices(res.data.smartDevices);
            setTotalCount(res.data.totalCount);
        }).catch(err => {
            console.log(err)
        });
    }

    const handleChangePage = (event, newPage) => {
        setPage(newPage);
    };

    return (
        <Box sx={{width:"100%", height:"100%", backgroundColor:"#DBDDEB"}}>
            <Container  maxWidth="xl" sx={{display: "flex",
                flexDirection: "row",
                justifyContent: "space-between",
                alignItems: "center",
                mt:"10px",
            }} >
                <Container maxWidth="xl" sx={{display: "flex", flexDirection: "column"}}>
                    <Typography mb={1} align="left" sx={{fontSize:"40px", fontWeight:"600"}}>{smartHome.name}</Typography>
                    <Container disableGutters sx={{display:"flex", flexDirection:"row"}}>
                        <LocationOn sx={iconStyle} /><Typography sx={textStyle}>{smartHome.address}, {smartHome.city.name}, {smartHome.city.country}</Typography>
                    </Container>
                </Container>

                <Container sx={{display: "flex", flexDirection: "row", height:"70px"}}>
                    <Container sx={{display: "flex", flexDirection: "column", alignItems:"center"}}>
                        <Typography sx={{fontSize:"18px", fontWeight:"600"}}>Type</Typography>
                        <Typography sx={{fontSize:"25px", fontWeight:"600", color:"#343F71", mt:"5px"}}>{smartHome.type == 0 ? "House" : "Flat"}</Typography>
                    </Container>
                    <div style={{ height: "100%", borderRight: "2px solid #FBC40E", margin: "0 10px" }}></div>
                    <Container sx={{display: "flex", flexDirection: "column", alignItems:"center"}}>
                        <Typography sx={{fontSize:"18px", fontWeight:"600"}}>Area</Typography>
                        <Typography sx={{fontSize:"25px", fontWeight:"600", color:"#343F71"}}>{smartHome.area}m<sup>2</sup></Typography>
                    </Container>
                    <div style={{ height: "100%", borderRight: "2px solid #FBC40E", margin: "0 10px" }}></div>
                    <Container sx={{display: "flex", flexDirection: "column", alignItems:"center"}}>
                        <Typography sx={{fontSize:"18px", fontWeight:"600"}}>Floor</Typography>
                        <Typography sx={{fontSize:"25px", fontWeight:"600", color:"#343F71", mt:"5px"}}>{smartHome.numberOfFloors}</Typography>
                    </Container>
                </Container>

            </Container>



            <Box sx={{ width: "98%", height: "3px", borderRadius: "15px", background: `#343F71`, margin:"10px auto"}}></Box>

            <Container maxWidth="xl" sx={{display: "flex", flexDirection: "row", justifyContent: "space-between", alignItems: "center", mt:"10px"}}>
                <Box sx={{background:"white", borderRadius:"5px", padding:"2px 5px", display:"flex", flexDirection:"row", height:"20px"}}>
                    <Typography sx={{fontSize:"15px", fontWeight:"600"}}>All devices</Typography>
                    <div style={{height:"100%", borderRight:"2px solid gray", margin:"0 10px"}}></div>
                    <div style={{height:"60%", borderRight:"4px solid #676E79", borderRadius:"2px", margin:"auto 5px"}}></div>
                    <Typography sx={{fontSize:"15px", fontWeight:"600"}}>Indoor home devices</Typography>
                    <div style={{height:"100%", borderRight:"2px solid gray", margin:"0 10px"}}></div>
                    <div style={{height:"60%", borderRight:"4px solid #F43F5E", borderRadius:"2px", margin:"auto 5px"}}></div>
                    <Typography sx={{fontSize:"15px", fontWeight:"600"}}>Outdoor home devices</Typography>
                    <div style={{height:"100%", borderRight:"2px solid gray", margin:"0 10px"}}></div>
                    <div style={{height:"60%", borderRight:"4px solid #2691D9", borderRadius:"2px", margin:"auto 5px"}}></div>
                    <Typography sx={{fontSize:"15px", fontWeight:"600"}}>High power devices</Typography>

                </Box>
                <Button sx={buttonStyle}><Add sx={{marginX:"5px", color:"white" }} fontSize="inherit"/><Typography sx={typoStyle}>Add new device</Typography></Button>
            </Container>

            <Container maxWidth="xl" style={{ position: 'relative' }}>
                {renderPanel()}
                <TablePagination
                    component="div"
                    count={totalCount}
                    page={page}
                    onPageChange={handleChangePage}
                    rowsPerPage={rowsPerPage}
                    rowsPerPageOptions={[8]}
                    style={{ position: 'fixed', bottom: 10, right: 30}}
                />
            </Container>

        </Box>
    )

}

export default SmartHomeMain;