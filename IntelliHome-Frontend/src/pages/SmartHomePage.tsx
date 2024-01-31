import {useParams} from "react-router-dom";
import SmartHomeMain from "../components/SmartHome/SmartHomeMain.tsx";

const SmartHomePage = () => {
    const { id } = useParams();
    return (
        <><SmartHomeMain smartHomeId={id}/></>
    )
}

export default SmartHomePage;