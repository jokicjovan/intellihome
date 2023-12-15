import {useEffect, useState} from "react";
import SmartDeviceReportAction from "../Shared/SmartDeviceReportAction.tsx";
import axios from "axios";
import {environment} from "../../../../utils/Environment.ts";

interface GateReportProps {
    environment: boolean
}

const GateReport = ({ device}) => {
    const [gate, setGate] = useState(device);
    const [startDate, setStartDate] = useState(new Date());
    const [endDate, setEndDate] = useState(new Date());
    const [user, setUser] = useState("");
    const [historicalData, setHistoricalData] = useState([]);

    useEffect(() => {
        axios.get(environment + `/api/VehicleGate/GetHistoricalActionData?Id=${gate.id}&From=${startDate.toISOString()}&To=${endDate.toISOString()}`).then(res => {
                // setHistoricalData(res.data)
                let data = []
                res.data.forEach((entry) => {
                    data.push({action: entry.action, by: entry.actionBy, date: new Date(entry.timestamp)})
                })
                // console.log(data)
                setHistoricalData(data.reverse())
            }
        ).catch(err => {
            console.log(err)
        });
    }, [device.id, startDate, endDate, user]);


    return <>
        <SmartDeviceReportAction
            inputData={historicalData}
            setParentStartDate={setStartDate}
            setParentEndDate={setEndDate}
            setParentUser={setUser}
        />
    </>
}

export default GateReport