import {useParams} from "react-router-dom";
import SmartHomeMain from "../components/SmartHome/SmartHomeMain.tsx";
import SmartDeviceMain from "../components/SmartDevices/Control/Shared/SmartDeviceMain";
import {Box} from "@mui/material";

const SmartDeviceHome = () => {
    const { id } = useParams();
    return (
        <><Box
            sx={{display: "flex", flexDirection: "column", width: "100%", backgroundColor: "#DBDDEB", padding: "20px"}}><SmartDeviceMain /*smartDeviceId={id}*//></Box></>
    )
}

export default SmartDeviceHome;