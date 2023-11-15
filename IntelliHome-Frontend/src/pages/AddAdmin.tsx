import {Box, Divider, Typography} from "@mui/material";
import AdminCardListItem from "../components/AddAdmin/AdminCardListItem";
import AdminRegistration from "../components/AddAdmin/AdminRegistration";

const AddAdmin = () => {
    return <Box
        sx={{display: "flex", flexDirection: "column", width: "100%", backgroundColor: "#DBDDEB", padding: "20px"}}>
        <Typography sx={{fontSize: "45px", fontWeight: "600", marginBottom: "25px"}}>Admins</Typography>
        <Box sx={{display: "flex", flexDirection: "row"}}>

            <Box sx={{width: "60%", height: "100%", bgColor: "red"}}>
                <AdminCardListItem></AdminCardListItem>
            </Box>
            <Divider orientation="vertical" flexItem/>
            <Box sx={{width: "40%", height: "100%", bgColor: "red", display: "flex", justifyContent: "center"}}>
                <AdminRegistration/>
            </Box>
        </Box>

    </Box>
}

export default AddAdmin;