import {Box, Button, InputLabel, Link, TextField, Typography} from "@mui/material";
import React, {useState} from "react";
import {useNavigate} from "react-router-dom";
import axios from "axios";
import {environment} from "../../security/Environment";


const SignInForm = () => {
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
    const [username, setUsername] = useState("")
    const [password, setPassword] = useState("")
    const [error, setError] = useState("")
    const navigate = useNavigate()


    function submitHandler(event: any) {
        event.preventDefault()
        axios.post(environment + `/api/User/login`, {
            username: username,
            password: password
        }).then(res => {
            if (res.status === 200) {
                navigate(0)
            }
        }).catch((error) => {
            console.log(error)
            if (error.response?.status !== undefined && error.response.status === 404) {
                setError("Invalid username or password!");
            } else if (error.response?.status !== undefined && error.response.status === 400) {
                setError("Invalid input!");
            } else {
                setError("An error occurred!");
            }
        });
    }
    return <Box component="form" onSubmit={submitHandler} sx={{display:"flex",width:"100%",justifyContent:"center",flexDirection:"column"}}>
        <Typography mb={1} align="center" sx={{fontSize:"48px",fontWeight:"600"}}>Sign In</Typography>
        <TextField id="username" placeholder="Username" sx={styled} onChange={(e) => {
            setUsername(e.target.value)
        }}></TextField>
        <TextField id="password" type="password" variant="outlined"  placeholder="Password" sx={styled} onChange={(e) => {
            setPassword(e.target.value)
        }}></TextField>
        <div>
            <InputLabel  style={{color: "red",textAlign:"center"}}>{error}</InputLabel>
        </div>
        <Button type="submit" sx={{backgroundColor:"#FBC40E", width:"400px",fontSize:"22px",fontWeight:"600",paddingY:"10px",margin:"15px auto",borderRadius:"15px", ':hover':{backgroundColor:"#EDB90D"}}}>Sign In</Button>
        <Typography align="center" sx={{fontSize:"18px",fontWeight:"600"}}>Don't have an account?</Typography>
        <Link  onClick={()=>{navigate("/signup")}}  align="center" sx={{fontSize:"20px",color:"#343F71",fontWeight:"600", ":hover":{cursor:"pointer"}}}>Sign up</Link>
    </Box>
}

export default SignInForm;