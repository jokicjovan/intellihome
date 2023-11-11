import {Box, Button, InputLabel, Link, TextField, Typography} from "@mui/material";
import React, {useState} from "react";
import {useNavigate} from "react-router-dom";
import axios from "axios";
import {environment} from "../security/Environment";


const SuccessfullRegistrationContainter = () => {
    const navigate = useNavigate()

    return <Box sx={{display:"flex",width:"100%",justifyContent:"center",flexDirection:"column"}}>
        <Typography mb={3} align="center" sx={{fontSize:"56px",fontWeight:"600"}}>Congratulations!</Typography>
        <Typography align="center" sx={{width:"70%",fontSize:"24px",fontWeight:"600",margin:"15px auto"}}>Your account is successfully created.
            To start using IntelliHome you need to
            verify your email address.</Typography>
        <Link  onClick={()=>{navigate("/signin")}}  align="center" sx={{fontSize:"24px",color:"#343F71",fontWeight:"600", ":hover":{cursor:"pointer"}}}>Sign in</Link>
    </Box>
}

export default SuccessfullRegistrationContainter;