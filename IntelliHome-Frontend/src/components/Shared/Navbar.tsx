import {Box, Button, Typography} from "@mui/material";
import {useContext} from "react";
import {AuthContext} from "../../security/AuthContext";
import {Group, Home} from "@mui/icons-material";
import {useNavigate} from "react-router-dom";

const Navbar = () => {
    const { role} = useContext(AuthContext);
    const navigate=useNavigate();
    const buttonStyle={width:"100%",color:"#FBC40EFF", justifyContent:"flex-start", paddingLeft:"15px", fontSize:"35px",textTransform:"None"}
    const typoStyle={color:"white", fontSize:"30px"}
    return <>
        <Box style={{height:"100vh", width:"300px",minWidth:"300px",backgroundColor:"#343F71FF"}}>
            {role==="Admin" &&<><Button onClick={()=>navigate("/home")}  sx={buttonStyle}><Home sx={{marginRight:"10px"}} fontSize="inherit"/><Typography sx={typoStyle}>Home</Typography></Button></>}
            {role==="Admin" &&<><Button onClick={()=>navigate("/addAdmin")}  sx={buttonStyle}><Group sx={{marginRight:"10px"}} fontSize="inherit"/><Typography sx={typoStyle}>Add Admin</Typography></Button></>}
            {role==="User" &&<><Button onClick={()=>navigate("/userHome")}  sx={buttonStyle}><Home sx={{marginRight:"10px"}} fontSize="inherit"/><Typography sx={typoStyle}>Home</Typography></Button></>}
        </Box>
    </>
}

export default Navbar;