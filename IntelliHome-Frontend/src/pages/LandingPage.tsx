import {Box, Button, Grid, Typography} from "@mui/material";
import {useNavigate} from "react-router-dom";

const LandingPage=()=>{

    const navigate = useNavigate()
    return <><Box sx={{width:"100%",height:"100%",position:"absolute",backgroundImage:"url(/backgroundLandingPage.png)", backgroundSize: 'cover',}}>
        <Box width="100%" height="5%" marginTop="15px" display="flex" justifyContent="flex-end">
            <Button onClick={()=>navigate("/signin")}  sx={{fontSize:"20px",marginX:"10px",paddingX:"20px" ,color:"#343F71FF",border:"3px solid #343F71FF", borderRadius:"10px",backgroundColor:"rgba(52,63,113,0.1)",':hover':{backgroundColor:"#2c3770",color:"white"}}} >Sign In</Button>
            <Button onClick={()=>navigate("/signup")} sx={{fontSize:"20px",marginX:"10px",paddingX:"20px",backgroundColor:"#343F71FF",color:"white", borderRadius:"10px",':hover':{backgroundColor:"#2c3770"}}} >Sign Up</Button>
        </Box>
        <Grid container  direction="row" width="100%" height="90%"
              justifyContent="center"
              alignItems="center" spacing={1}>


            <Grid item xs={6} display="flex" justifyContent="center" alignItems="center" flexDirection="column">
                <Typography fontSize="45px" fontWeight="800" display="flex" maxWidth="600px" flexWrap="wrap">We Partner with Industry Leaders to provide amazing Smarthome platform</Typography>
                <Typography width="100%" textAlign="left" fontSize="22px" fontWeight="400" display="flex" maxWidth="600px" marginLeft={"450px"} flexWrap="wrap">Join us today!</Typography>
                <Button onClick={()=>navigate("/signup")}  sx={{fontSize:"30px",marginX:"10px",marginTop:"40px",paddingX:"40px",fontWeight:"800" ,color:"#343F71FF",border:"3px solid #343F71FF", borderRadius:"10px",backgroundColor:"rgba(52,63,113,0.1)",':hover':{backgroundColor:"#2c3770",color:"white"}}} >Sign Up</Button>

            </Grid>
            <Grid item xs={6} display="flex" justifyContent="center">
                <img height="600px" src="/iotSystem.png"/>
            </Grid>
        </Grid>
    </Box>
    </>
}

export default  LandingPage;