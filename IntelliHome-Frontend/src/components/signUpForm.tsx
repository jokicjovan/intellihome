import {Box, Button, Link, TextField, Typography} from "@mui/material";
import {CloudUpload} from "@mui/icons-material";
import {useNavigate} from "react-router-dom";
import {useMutation, useQueryClient} from "react-query";
import axios from "axios";
import {environment} from "../security/Environment";


const SignUpForm = () => {
    const navigate = useNavigate()
    const queryClient = useQueryClient()
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
    const registrationMutation = useMutation({
        mutationFn: (data:any) => {
            return axios.post(environment+'/api/User/register', data).then(()=>{navigate("/successfullRegistration")})
        },
    })
    const handleSignUp = async (event) => {
        event.preventDefault();

        const formData = {
            firstName: event.target.firstName.value,
            lastName: event.target.lastName.value,
            email: event.target.email.value,
            username: event.target.username.value,
            password: event.target.password.value,
        };

        // Call the mutation function with the form data
        registrationMutation.mutate(formData);
    };
    return <Box component="form" onSubmit={handleSignUp}  sx={{display:"flex",width:"100%",justifyContent:"center",flexDirection:"column"}}>
        <Typography mb={1} align="center" sx={{fontSize:"48px",fontWeight:"600"}}>Sign Up</Typography>
        <TextField name="firstName"   placeholder="First Name" sx={styled}></TextField>
        <TextField name="lastName" placeholder="Last Name" sx={styled}></TextField>
        <TextField name="email" placeholder="Email" sx={styled} mb={4}></TextField>
        <Box height="35px"/>
        <TextField name="username" placeholder="Username" sx={styled}></TextField>
        <TextField name="password" type="password" variant="outlined"  placeholder="Password" sx={styled}></TextField>
        <TextField name="confirmPassword" type="password" variant="outlined"  placeholder="Confirm Password" sx={styled}></TextField>
        <Button startIcon={<CloudUpload/>} sx={{backgroundColor:"transparent", textTransform:"none", width:"400px",fontSize:"24px",fontWeight:"600",paddingY:"10px",margin:"15px auto",borderRadius:"15px", ':hover':{backgroundColor:"transparent"}}}>Upload profile picture</Button>
        <Button type="submit" sx={{backgroundColor:"#FBC40E", width:"400px",fontSize:"22px",fontWeight:"600",paddingY:"10px",margin:"15px auto",borderRadius:"15px", ':hover':{backgroundColor:"#EDB90D"}}}>Sign In</Button>
        <Typography align="center" sx={{fontSize:"18px",fontWeight:"600"}}>Already have an account?</Typography>
        <Link onClick={()=>{navigate("/signin")}}align="center" sx={{fontSize:"20px",color:"#343F71",fontWeight:"600", ":hover":{cursor:"pointer"}}}>Sign in</Link>
    </Box>
}

export default SignUpForm;