import {Box, Divider, Grid, TablePagination, Typography} from "@mui/material";
import AdminCardListItem from "../components/AddAdmin/AdminCardListItem";
import AdminRegistration from "../components/AddAdmin/AdminRegistration";
import {useEffect, useState} from "react";
import {useQuery} from "react-query";
import axios from "axios";
import {environment} from "../utils/Environment.ts";

const AddAdmin = () => {

    const [page, setPage] = useState(0);
    const [totalCount, setTotalCount] = useState(0);
    const [admins, setAdmins] = useState([]);
    const [pagePerRow, setPagePerRow] = useState(5);
    const handleChangePage = (
        event: React.MouseEvent<HTMLButtonElement> | null,
        newPage: number,
    ) => {
        setPage(newPage);
    };

    const handleChangeRowsPerPage = (
        event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
    ) => {
        setPagePerRow(parseInt(event.target.value, 10));
        setPage(0);
    };
    const _ = useQuery({
        queryKey: ['adminList'],
        queryFn: () => {
            return axios.get(environment + '/api/User/allAdmins').then((res) => {
                setTotalCount(res.data.totalCount)
                setAdmins(res.data.admins)
            })
        },
    })
    return <Box
        sx={{display: "flex", flexDirection: "column", width: "100%", backgroundColor: "#DBDDEB", padding: "20px"}}>
        <Typography sx={{fontSize: "45px", fontWeight: "600", marginBottom: "25px"}}>Admins</Typography>
        <Grid container spacing={1} sx={{padding:"10px"}}>
            <Grid item xs={7} >
            <div style={{display:"flex",flexDirection:"column", minHeight:"762px"}}>
                {admins.slice(page*pagePerRow, (page+1)*pagePerRow).map(admin =>  <AdminCardListItem key={admin.email} firstName={admin.firstName} lastName={admin.lastName} email={admin.email}/>)}

                <TablePagination sx={{marginTop:"auto"}} className="paginator" component="div" page={page} rowsPerPage={pagePerRow}
                                 onPageChange={handleChangePage} onRowsPerPageChange={handleChangeRowsPerPage} rowsPerPageOptions={[5, 25, 50, 100]}
                                 count={totalCount}/>
            </div>
            </Grid>
            <Grid item xs={1} sx={{display:"flex",justifyContent:"center"}} ><Divider sx={{margin:"0 auto"}} orientation="vertical"/></Grid>
            <Grid item xs={4}>
            <Box sx={{width:"100%", display: "flex", justifyContent: "center"}}>
                <AdminRegistration/>
            </Box>
            </Grid>
        </Grid>

    </Box>
}

export default AddAdmin;