import {Box, Button, Typography} from "@mui/material";
import {useContext} from "react";
import {AuthContext} from "../../security/AuthContext";
import {Group, Home, Power} from "@mui/icons-material";
import {useNavigate} from "react-router-dom";
import axios from "axios";
import {environment} from "../../utils/Environment.ts";

const Navbar = () => {
    const { role} = useContext(AuthContext);
    const navigate=useNavigate();
    const handleLogoutClick = (event) => {
        event.preventDefault()

        axios.post(environment + `/api/User/logout`)
            .then(res => {
                if (res.status === 200){
                    navigate(0);
                }
            }).catch((error) => {
            console.log(error);
        });
    };
    const buttonStyle={width:"100%",color:"#FBC40EFF", justifyContent:"flex-start", paddingLeft:"15px", fontSize:"35px",textTransform:"None"}
    const typoStyle={color:"white", fontSize:"30px"}
    return <>
        <Box style={{height:"100vh", width:"360px",minWidth:"360px",backgroundColor:"#343F71FF" }}>
            {role==="Admin" &&<><Button onClick={()=>navigate("/home")}  sx={buttonStyle}><Home sx={{marginRight:"10px"}} fontSize="inherit"/><Typography sx={typoStyle}>Home</Typography></Button></>}
            {role==="Admin" &&<><Button onClick={()=>navigate("/addAdmin")}  sx={buttonStyle}><Group sx={{marginRight:"10px"}} fontSize="inherit"/><Typography sx={typoStyle}>Add Admin</Typography></Button></>}
            {role==="Admin" &&<><Button onClick={()=>navigate("/consumption")}  sx={buttonStyle}><Power sx={{marginRight:"10px"}} fontSize="inherit"/><Typography sx={typoStyle}>Consumption</Typography></Button></>}
            {role==="User" &&<><Button onClick={()=>navigate("/userHome")}  sx={buttonStyle}><Home sx={{marginRight:"10px"}} fontSize="inherit"/><Typography sx={typoStyle}>Home</Typography></Button></>}
            <Box position="absolute" bottom="0">
                <Box padding="10px" display="flex"  alignItems="center" flexDirection="row">
                    <img src="/backgroundSign.png" width="50px" height="50px" style={{objectFit:"cover",borderRadius:"100px",border:"5px solid #FBC40EFF"}}/>
                    <Box display="flex" justifyContent="center" flexDirection="column" textAlign="left" ml={1} mt={1} mb={0}>
                        <Typography textAlign="left" fontSize="22px" fontWeight="600" color="white">Vukasin Bogdanovic</Typography>
                        <Typography onClick={handleLogoutClick} textAlign="left" fontSize="16px" fontWeight="600" color="#FBC40EFF" sx={{":hover":{cursor:"pointer"}}}>Logout</Typography>
                    </Box>
                </Box>
            </Box>
        </Box>
    </>
}

export default Navbar;