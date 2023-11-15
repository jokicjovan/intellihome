import {Box, Typography} from "@mui/material";

const AdminCardListItem = () => {
    return <>
        <Box sx={{display:"flex", justifyContent:"space-between",bgColor:"white",maxWidth:"800px",alignItems:"center",backgroundColor:"white",padding:"10px",borderRadius:"15px",height:"90px"}}>
            <Box display="flex"  alignItems="center" flexDirection="row">
                <img src="src/assets/backgroundSign.png" width="80px" height="80px" style={{objectFit:"cover",borderRadius:"7px",border:"5px solid #FBC40EFF"}}/>
                <Box display="flex" flexDirection="column" textAlign="left" ml={1}>
                    <Typography textAlign="left" fontSize="30px" fontWeight="800">Petar</Typography>
                    <Typography textAlign="left" fontSize="30px" fontWeight="800">Petrovic</Typography>
                </Box>
            </Box>
            <Typography color="#676767">vukasin@example.com</Typography>
        </Box>
    </>
}

export default AdminCardListItem;