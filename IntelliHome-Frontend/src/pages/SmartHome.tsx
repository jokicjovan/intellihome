import {useParams} from "react-router-dom";
import SmartHomeMain from "../components/SmartHome/SmartHomeMain.tsx";

const SmartHome = () => {
    const { id } = useParams();
    return (
        <><SmartHomeMain smartHomeId={id}/></>
    )
}

export default SmartHome;