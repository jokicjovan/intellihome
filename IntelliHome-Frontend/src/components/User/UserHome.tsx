import {Box, Button, Container, Grid, TablePagination, TextField, Typography} from "@mui/material";
import {Add} from "@mui/icons-material";
import React, {useEffect} from "react";
import axios from "axios";
import SmartHomeCard from "./SmartHomeCard.tsx";
import {environment} from "../../security/Environment.tsx";

const UserHome=()=>{
    const buttonStyle={backgroundColor:"#FBC40E", boxShadow: "0 2px 5px rgba(0, 0, 0, 0.2)", width:"80px",  height:"30px", fontSize:"20px",fontWeight:"600",margin:"15px",borderRadius:"5px", ':hover':{backgroundColor:"#EDB90D"}, textTransform: "none"}
    const typoStyle={color:"white", fontSize:"20px", marginRight:"10px"}
    const [page, setPage] = React.useState(0);
    const [totalCount, setTotalCount] = React.useState(0);
    const [rowsPerPage, setRowsPerPage] = React.useState(8);
    const [smartHomes, setSmartHomes]=React.useState([]);
    const [search, setSearch]=React.useState("");
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
                <Grid container sx={{bx:3, mt:1, height:"100%"}} columnSpacing={5}>
                    {smartHomes.map(item => (
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
            <Button onClick={()=>console.log("stisnuo")} sx={buttonStyle}><Add sx={{marginX:"5px", color:"white" }} fontSize="inherit"/><Typography sx={typoStyle}>Add</Typography></Button>
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



    </Box>
}
export default UserHome;