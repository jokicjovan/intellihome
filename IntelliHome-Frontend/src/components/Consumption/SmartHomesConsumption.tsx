import React, {useEffect, useState} from "react";
import axios from "axios";
import {environment} from "../../utils/Environment.ts";
import {Box, Container, Dialog, Grid, TablePagination, TextField} from "@mui/material";
import SmartHomeConsumptionCard from "./SmartHomeConsumptionCard.tsx";
import SmartHomeConsumptionReport from "./SmartHomeConsumptionReport.tsx";

const SmartHomesConsumption = () => {
    const [smartHomes, setSmartHomes]=React.useState([]);
    const [selectedSmartHomeId, setSelectedSmartHomeId]=React.useState("");
    const [page, setPage] = React.useState(0);
    const [totalCount, setTotalCount] = React.useState(0);
    const [rowsPerPage, setRowsPerPage] = React.useState(8);
    const [search, setSearch]=React.useState("");
    const [openModal, setOpenModal] = useState(false);

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
        axios.get(environment + `/api/SmartHome/GetAllSmartHomesPaged?PageNumber=${page + 1}&PageSize=${rowsPerPage}&Search='${search == "" ? "" : search}'`).then(res => {
            setTotalCount(res.data.totalCount);
            setSmartHomes(res.data.smartHomes);
        }).catch(err => {
            console.log(err)
        });
    }

    const searchHandler = (event) => {
        setSearch(event.target.value);
    }

    useEffect( ()=>{
        getSmartHomes()
    },[page, rowsPerPage, search])

    const onViewConsumptionClick = (selectedSmartHomeId) => {
        setSelectedSmartHomeId(selectedSmartHomeId);
        setOpenModal(true);
    }

    const renderPanel = () => {
        return <>
            {smartHomes.length===0 ? <p>No smart homes to show...</p> : <div>
                <Grid container sx={{ boxSizing: 'border-box', mt: 1, height: '100%', width: '100%', px: 3 }}>
                    {smartHomes.map((item) => (
                        <Grid item key={item.id} xs={12} sm={6} md={4} lg={3}>
                            <SmartHomeConsumptionCard smartHome={item} onButtonClick={onViewConsumptionClick}/>
                        </Grid>
                    ))}
                </Grid>
            </div>
            }
        </>
    };

    return <Box>
        <Container  maxWidth="xl" sx={{display: "flex",
            flexDirection: "row",
            justifyContent: "space-between",
            alignItems: "center",
            mt:"10px",
        }} >
            <TextField id="search_bar" onChange={searchHandler} label="Search" variant="outlined" sx={searchStyle} inputProps={{
                style: {
                    padding: 10,
                }
            }}  focused/>
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

        <Dialog open={openModal} onClose={() => setOpenModal(false)}
                sx={{
                    '& .MuiDialog-paper': {
                        minWidth: '1000px',
                    },
                }}>
            <Box sx={{
                width: "100%",
                height: "100%",
                backgroundColor: "white",
                display: "flex",
                flexDirection: "column"
            }}>
                <SmartHomeConsumptionReport smartHomeId={selectedSmartHomeId}/>
            </Box>
        </Dialog>
    </Box>
}


export default SmartHomesConsumption