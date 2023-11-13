import {Box, Button, Container, Dialog, Grid, TablePagination, TextField, Typography} from "@mui/material";
import {Add} from "@mui/icons-material";
import React, {useEffect} from "react";
import axios from "axios";
import SmartHomeCard from "./SmartHomeCard.tsx";
import {environment} from "../../security/Environment.tsx";
import SmartHomeCreatingMap from "./SmartHomeCreatingMap.tsx";

const UserHome=()=>{
    const buttonStyle={backgroundColor:"#FBC40E", boxShadow: "0 2px 5px rgba(0, 0, 0, 0.2)", width:"80px",  height:"30px", fontSize:"20px",fontWeight:"600",margin:"15px",borderRadius:"5px", ':hover':{backgroundColor:"#EDB90D"}, textTransform: "none"}
    const typoStyle={color:"white", fontSize:"20px", marginRight:"10px"}
    const [page, setPage] = React.useState(0);
    const [totalCount, setTotalCount] = React.useState(0);
    const [rowsPerPage, setRowsPerPage] = React.useState(8);
    const [smartHomes, setSmartHomes]=React.useState([]);
    const [search, setSearch]=React.useState("");
    const [openModal, setOpenModal] = React.useState(false);
    const [modalPage, setModalPage] = React.useState(0);
    const [button1Text, setButton1Text] = React.useState("Cancel");
    const [button2Text, setButton2Text] = React.useState("Next");
    const [line1Color, setLine1Color] = React.useState("#343F71FF")
    const [line2Color, setLine2Color] = React.useState("#DBDDEB")
    const [line3Color, setLine3Color] = React.useState("#DBDDEB")

    const searchStyle = {
        '& label.Mui-focused': {
            color: '#343F71FF',
            fontWeight: 'bold',
        },
        '& .MuiOutlinedInput-root': {
            '&.Mui-focused fieldset': {
                borderColor: '#343F71FF',
                borderRadius: '10px',
            },
            borderRadius: '10px',
        },
        margin: '15px auto',
        width: '50%',
        backgroundColor: '#DBDDEB',
        borderRadius: '10px',
        border: '10px',
        '& input': {
            fontSize: '16px',
            fontWeight: 'bold',
        },
    };

    const handleChangePage = (event, newPage: number) => {
        setPage(newPage);
    };
    const handleChangeRowsPerPage = (event) => {
        setRowsPerPage(parseInt(event.target.value, 8));
        setPage(0);
    };

    const getSmartHomes = () => {
        console.log(search == "")
        axios.get(environment + `/api/SmartHome/GetSmartHomesForUser?PageNumber=${page + 1}&PageSize=${rowsPerPage}&Search='${search == "" ? "" : search}'`).then(res => {
            console.log(res.data);
            setTotalCount(res.data.totalCount);
            setSmartHomes(res.data.smartHomes);
        }).catch(err => {
            console.log(err)
        });
    }

    useEffect( ()=>{
        getSmartHomes()
    },[page, rowsPerPage, search])

    const renderPanel = () => {
        return <>
            {smartHomes.length===0 ? <p>No smart homes to show...</p> : <div>
                <Grid container sx={{ boxSizing: 'border-box', mt: 1, height: '100%', width: '100%', px: 3 }}>
                    {smartHomes.map((item) => (
                        <Grid item key={item.id} xs={12} sm={6} md={4} lg={3}>
                            <SmartHomeCard data={item} />
                        </Grid>
                    ))}
                </Grid>
            </div>
            }
        </>
    };

    const searchHandler = (event) => {
        setSearch(event.target.value);
    }

    const handleCloseModal = () => {
        setOpenModal(false);
    };

    const button1Handler = () => {
        if(modalPage===0){
            setOpenModal(false);
        }
        else if(modalPage===1){
            setModalPage(0);
            setButton1Text("Cancel");
            setLine2Color("#DBDDEB");
        }
        else if(modalPage===2){
            setModalPage(1);
            setButton2Text("Next");
            setLine3Color("#DBDDEB");
        }
    }

    const button2Handler = () => {
        if(modalPage===0) {
            setModalPage(1);
            setButton1Text("Back");
            setLine2Color("#343F71FF");
        }
        else if(modalPage===1){
            setModalPage(2);
            setButton2Text("Finish")
            setLine3Color("#343F71FF");
        }
        else if(modalPage===2){
            setOpenModal(false);
            //TODO: Create new smart home
        }
    }

    const modalContent = (
        <Box sx={{ width: "50vw", height: "60vh", backgroundColor: "white", borderRadius: "10px", padding: "20px", display: "flex", flexDirection: "column", position: "relative" }}>
            <Typography sx={{ fontSize: "30px", fontWeight: "600", margin: "10px" }}>Add new property</Typography>
            <Box sx={{ display: "flex", flexDirection: "row", justifyContent: "space-between", mb:"20px"}}>
                <Box sx={{ width: "32%", height: "7px", borderRadius: "15px", background: `${line1Color}` }}></Box>
                <Box sx={{ width: "32%", height: "7px", borderRadius: "15px", background: `${line2Color}` }}></Box>
                <Box sx={{ width: "32%", height: "7px", borderRadius: "15px", background: `${line3Color}` }}></Box>
            </Box>
            {/* Content */}
            <Box sx={{ flex: 1, position: 'relative', overflow: 'hidden' }}>
                {modalPage === 0 && <SmartHomeCreatingMap />}
            </Box>
            {/* Buttons */}
            <Box sx={{ display: "flex", justifyContent: "flex-end", alignItems: "center", mt:"30px" }}>
                <Button onClick={button1Handler} sx={{ marginRight: "20px", background: "white", width: "7vw", height: "5vh", color: "black", border: '1px solid #FBC40E', fontWeight: "500", textTransform: "none", fontSize: "1.4rem" }}>{button1Text}</Button>
                <Button onClick={button2Handler} sx={{ background: "#FBC40E", width: "7vw", height: "5vh", fontWeight: "500", textTransform: "none", fontSize: "1.4rem" }}>{button2Text}</Button>
            </Box>
        </Box>
    );









    return <Box sx={{width:"100%", height:"100%", backgroundColor:"#DBDDEB"}}>
        <Container  maxWidth="xl" sx={{display: "flex",
            flexDirection: "row",
            justifyContent: "space-between",
            alignItems: "center",
            mt:"10px",
        }} >
            <Typography mb={1} align="left" sx={{fontSize:"40px", fontWeight:"600", margin: "5px 10px"}}>Properties</Typography>
            <TextField id="search_bar" onChange={searchHandler} label="Search" variant="outlined" sx={searchStyle} inputProps={{
                style: {
                    padding: 10,
                }
            }}  focused/>
            <Button onClick={()=>setOpenModal(true)} sx={buttonStyle}><Add sx={{marginX:"5px", color:"white" }} fontSize="inherit"/><Typography sx={typoStyle}>Add</Typography></Button>
        </Container>

        <Container maxWidth="xl" style={{ position: 'relative' }}>
            {renderPanel()}
            <TablePagination
                component="div"
                count={totalCount}
                page={page}
                onPageChange={handleChangePage}
                rowsPerPage={rowsPerPage}
                onRowsPerPageChange={handleChangeRowsPerPage}
                rowsPerPageOptions={[8]}
                style={{ position: 'fixed', bottom: 10, right: 30}}
            />
        </Container>

        <Dialog  maxWidth="xl" open={openModal} onClose={handleCloseModal}>
            {modalContent}
        </Dialog>


    </Box>
}
export default UserHome;