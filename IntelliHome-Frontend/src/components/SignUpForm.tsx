import {Box, Button, Link, TextField, Typography} from "@mui/material";
import {CloudUpload} from "@mui/icons-material";


const SignUpForm = () => {
    const styled = {
        "& label.Mui-focused": {
            color: "#FBC40E"
        },
        "& .MuiOutlinedInput-root": {
            "&.Mui-focused fieldset": {
                borderColor: "#FBC40E",
                borderRadius:"10px"

            },
            borderRadius:"10px"
        },
        margin:"15px auto",width:"400px",backgroundColor:"white",borderRadius:"10px"

    }
    return <Box sx={{display:"flex",width:"100%",justifyContent:"center",flexDirection:"column"}}>
        <Typography mb={1} align="center" sx={{fontSize:"48px",fontWeight:"600"}}>Sign Up</Typography>
        <TextField  placeholder="First Name" sx={styled}></TextField>
        <TextField  placeholder="Last Name" sx={styled}></TextField>
        <TextField  placeholder="Email" sx={styled} mb={4}></TextField>
        <Box height="35px"/>
        <TextField  placeholder="Username" sx={styled}></TextField>
        <TextField type="password" variant="outlined"  placeholder="Password" sx={styled}></TextField>
        <TextField type="password" variant="outlined"  placeholder="Confirm Password" sx={styled}></TextField>
        <Button startIcon={<CloudUpload/>} sx={{backgroundColor:"transparent", textTransform:"none", width:"400px",fontSize:"24px",fontWeight:"600",paddingY:"10px",margin:"15px auto",borderRadius:"15px", ':hover':{backgroundColor:"transparent"}}}>Upload profile picture</Button>
        <Button sx={{backgroundColor:"#FBC40E", width:"400px",fontSize:"22px",fontWeight:"600",paddingY:"10px",margin:"15px auto",borderRadius:"15px", ':hover':{backgroundColor:"#EDB90D"}}}>Sign In</Button>
        <Typography align="center" sx={{fontSize:"18px",fontWeight:"600"}}>Already have an account?</Typography>
        <Link to="/signin" align="center" sx={{fontSize:"20px",color:"#343F71",fontWeight:"600"}}>Sign in</Link>
    </Box>
}

export default SignUpForm;