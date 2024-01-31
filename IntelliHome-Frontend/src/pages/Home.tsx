import UserHome from "../components/User/UserHome.tsx";
import {AuthContext} from "../security/AuthContext.tsx";
import {useContext} from "react";
import {AdminRoute} from "../security/AdminRoute";
import AdminHome from "../components/Admin/AdminHome.tsx";


const Home=()=>{
    const { role} = useContext(AuthContext);
    return (
        <>
            {role==="Admin" &&<AdminRoute><AdminHome></AdminHome></AdminRoute>}
            {role==="User" &&<UserHome/>}
        </>
    )
};

export default Home;