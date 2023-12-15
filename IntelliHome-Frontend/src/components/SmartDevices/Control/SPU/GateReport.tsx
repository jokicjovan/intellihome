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
        axios.get(environment + `/api/VehicleGate/GetHistoricalData?Id=${gate.id}&From=${startDate.toISOString()}&To=${endDate.toISOString()}`).then(res => {
                // setHistoricalData(res.data)
                let data = []
                res.data.forEach((entry) => {
                    let action: string
                    if(entry.isOpenedByUser)
                    {
                        if(entry.isOpen)
                            action = "Gate opened by user"
                        else
                            action = "Gate closed by user"
                    }
                    else
                    {
                        if(entry.isOpen)
                            action = "Gate opened by system"
                        else
                            action = "Gate closed by system"
                    }
                    data.push({action: action, by: entry.actionBy, date: new Date(entry.timestamp)})
                })
                // console.log(data)
                setHistoricalData(data)
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