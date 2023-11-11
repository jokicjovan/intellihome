import SignContainter from "../components/signContainter.tsx";
import SuccessfullRegistrationContainter from "../components/successfullRegistrationContainter";
import {Box, Container, CssBaseline, Icon, Link, Typography} from "@mui/material";
import React, {useEffect} from "react";
import {useNavigate, useSearchParams} from "react-router-dom";
import {CheckCircle, Verified} from "@mui/icons-material";
import {useMutation} from "react-query";
import axios from "axios";
import {environment} from "../security/Environment";


const SuccessfullActivation=()=>{

    const navigate = useNavigate()
    const [searchParams, setSearchParams] = useSearchParams();

    const activateMutation = useMutation({
        mutationFn: (code:string) => {
            return axios.post(environment+'/api/User/activateAccount?code='+code).then(()=>{})
        },
    })
    useEffect(() => {
        const code = searchParams.get('code');
        activateMutation.mutate(code);

    },[]);

    return <><Container  maxWidth="xl" sx={{display: "flex",
        justifyContent: "center",
        alignItems: "center",
        height: "100vh"
    }} >
        <CssBaseline/>
        <Box sx={{width:"100%",height: "55.625rem",display:"flex",flexDirection:"column", backgroundColor: "#DBDDEB", boxShadow:"0px 15px 30px 15px rgba(0, 0, 0, 0.25)",borderRadius:"52px",overflow:"hidden"}}>
            <CheckCircle sx={{margin:"20px auto", color:"#039F13", fontSize:"30rem"}}></CheckCircle>
            <Typography mb={3} align="center" sx={{fontSize:"56px",fontWeight:"600"}}>Congratulations!</Typography>
            <Typography align="center" sx={{width:"70%",fontSize:"24px",fontWeight:"600",margin:"15px auto"}}>Your account is successfully created.
                To start using IntelliHome you need to
                verify your email address.</Typography>
            <Link  onClick={()=>{navigate("/signin")}}  align="center" sx={{fontSize:"24px",color:"#343F71",fontWeight:"600", ":hover":{cursor:"pointer"}}}>Sign in</Link>

        </Box>
    </Container></>
}

export default  SuccessfullActivation;