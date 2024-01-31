import {Box, CircularProgress, Container, CssBaseline, Link, Typography} from "@mui/material";
import {useEffect, useRef, useState} from "react";
import {useNavigate, useSearchParams} from "react-router-dom";
import {CheckCircle, Error} from "@mui/icons-material";
import axios from "axios";
import {environment} from "../utils/Environment.ts";


const SuccessfulActivation=()=>{

    const navigate = useNavigate()
    const [searchParams] = useSearchParams();
    const [isLoading,setIsLoading]= useState(true);
    const canCallApi = useRef(true);

    const [isValid,setIsValid]= useState(false);
    const activateMutation = (code:string)=>{
            return axios.post(environment+'/api/User/activateAccount?code='+code).then(res => {
                if (res.status === 200){
                    setIsLoading(false);
                    setIsValid(true);
                }else{
                    setIsLoading(false);
                }}).catch(()=>{
                    setIsLoading(false);
            })
    }
    useEffect(() => {
        const code = searchParams.get('code')!;
        if (canCallApi.current) {
            activateMutation(code);
            canCallApi.current = false;
        }

    });

    return <><Container  maxWidth="xl" sx={{display: "flex",
        justifyContent: "center",
        alignItems: "center",
        height: "100vh"
    }} >
        <CssBaseline/>
        <Box sx={{width:"100%",height: "55.625rem",display:"flex",flexDirection:"column", backgroundColor: "#DBDDEB", boxShadow:"0px 15px 30px 15px rgba(0, 0, 0, 0.25)",borderRadius:"52px",overflow:"hidden"}}>
            {!isLoading && isValid && <><CheckCircle sx={{margin:"20px auto", color:"#039F13", fontSize:"30rem"}}></CheckCircle>
            <Typography mb={3} align="center" sx={{fontSize:"56px",fontWeight:"600"}}>Congratulations!</Typography>
            <Typography align="center" sx={{width:"70%",fontSize:"24px",fontWeight:"600",margin:"15px auto"}}>Your account is successfully created.
                To start using IntelliHome you need to
                verify your email address.</Typography>
            <Link  onClick={()=>{navigate("/signin")}}  align="center" sx={{fontSize:"24px",color:"#343F71",fontWeight:"600", ":hover":{cursor:"pointer"}}}>Sign in</Link></>}

            {!isLoading && !isValid && <>
                <Error sx={{margin:"20px auto", color:"red", fontSize:"30rem"}}></Error>
                <Typography mb={3} align="center" sx={{fontSize:"56px",fontWeight:"600"}}>Oops!</Typography>
                <Typography align="center" sx={{width:"70%",fontSize:"24px",fontWeight:"600",margin:"15px auto"}}>Something went wrong. Please check your email.</Typography>
                <Link mt={3} onClick={()=>{navigate("/signin")}}  align="center" sx={{fontSize:"24px",color:"#343F71",fontWeight:"600", ":hover":{cursor:"pointer"}}}>Sign in</Link></>}

            {isLoading &&
                <CircularProgress style={{color:"#343F71", margin:"auto",alignSelf:"center"}} />}


        </Box>
    </Container></>
}

export default  SuccessfulActivation;