import {Typography} from "@mui/material";
import UserHome from "../components/User/UserHome.tsx";
import {AuthContext} from "../security/AuthContext.tsx";
import {useContext} from "react";
import AdminHome from "../components/Admin/AdminHome.tsx";


const Home=()=>{
    const { role} = useContext(AuthContext);
    return (
        <>
            {role==="Admin" &&<AdminHome/>}
            {role==="User" &&<UserHome/>}
        </>
    )
};

export default Home;