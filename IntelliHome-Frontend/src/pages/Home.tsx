import {Typography} from "@mui/material";
import UserHome from "../components/User/UserHome.tsx";
import {AuthContext} from "../security/AuthContext.tsx";
import {useContext} from "react";


const Home=()=>{
    const { role} = useContext(AuthContext);
    return (
        <>
            {role==="Admin" &&<><Typography variant="h1">Admin Home</Typography></>}
            {role==="User" &&<UserHome/>}
        </>
    )
};

export default Home;