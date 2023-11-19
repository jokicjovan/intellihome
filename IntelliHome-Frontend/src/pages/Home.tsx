import {Typography} from "@mui/material";
import UserHome from "../components/User/UserHome.tsx";
import {AuthContext} from "../security/AuthContext.tsx";
import {useContext} from "react";
import AdminHome from "../components/Admin/AdminHome.tsx";
import {AdminRoute} from "../security/AdminRoute";


const Home=()=>{
    const { role} = useContext(AuthContext);
    return (
        <>
            {role==="Admin" &&<AdminRoute><AdminHome/></AdminRoute>}
            {role==="User" &&<UserHome/>}
        </>
    )
};

export default Home;