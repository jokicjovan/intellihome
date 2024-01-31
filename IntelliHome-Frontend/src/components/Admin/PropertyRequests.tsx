import {Box, Container, Grid, TablePagination, Typography} from "@mui/material";
import React, {useEffect} from "react";
import axios from "axios";
import SmartHomeApprovalCard from "./SmartHomeApprovalCart.tsx";
import {environment} from "../../utils/Environment.ts";


const PropertyRequests = () => {

    const [page, setPage] = React.useState(0);
    const [rowsPerPage, setRowsPerPage] = React.useState(8);
    const [totalCount, setTotalCount] = React.useState(0);
    const [smartHomes, setSmartHomes] = React.useState([]);
    const [openModal, setOpenModal] = React.useState(false);

    const handleChangePage = (event, newPage: number) => {
        setPage(newPage);
    };

    const renderPanel = () => {
        return <>
            {smartHomes.length===0 ? <p>No smart homes to approve...</p> : <div>
                <Grid container sx={{ boxSizing: 'border-box', mt: 1, height: '100%', width: '100%', px: 3 }}>
                    {smartHomes.map((item) => (
                        <Grid item key={item.id} xs={12} sm={6} md={4} lg={3}>
                            <SmartHomeApprovalCard data={item} onChangeHendler={onChangeHendler}/>
                        </Grid>
                    ))}
                </Grid>
            </div>
            }
        </>
    };

    const getSmartHomesForApproval = () => {
        axios.get(environment + `/api/SmartHome/GetSmartHomesForApproval?PageNumber=${page + 1}&PageSize=${rowsPerPage}`).then(res => {
            console.log(res.data);
            setTotalCount(res.data.totalCount);
            setSmartHomes(res.data.smartHomes);
        }).catch(err => {
            console.log(err)
        });
    }

    useEffect( ()=>{
        getSmartHomesForApproval()
    },[page, rowsPerPage])


    const onChangeHendler = (id) => {
        const newSmartHomes = smartHomes.filter((item) => item.id !== id);
        setSmartHomes(newSmartHomes);
    }


    return <Box sx={{width:"100%", height:"100%", backgroundColor:"#DBDDEB"}}>
        <Typography mb={1} align="left" sx={{fontSize:"40px", fontWeight:"600", margin: "5px 10px"}}>Property Requests</Typography>


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
}

export default PropertyRequests;