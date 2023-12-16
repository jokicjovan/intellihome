import {useEffect, useState} from "react";
import SmartDeviceReportAction from "../Shared/SmartDeviceReportAction.tsx";
import axios from "axios";
import {environment} from "../../../../utils/Environment.ts";
import dayjs from "dayjs";


const GateReport = ({ device, report }) => {
    const [gate, setGate] = useState(device);
    const [startDate, setStartDate] = useState(dayjs().subtract(24, "hour"));
    const [endDate, setEndDate] = useState(dayjs());
    const [user, setUser] = useState("");
    const [historicalData, setHistoricalData] = useState([]);

    useEffect(() => {
        console.log("GateReport useEffect");
        axios.get(environment + `/api/VehicleGate/GetHistoricalActionData?Id=${gate.id}&From=${startDate.toISOString()}&To=${endDate.toISOString()}`).then(res => {
                // setHistoricalData(res.data)
                let data = []
                res.data.forEach((entry) => {
                    data.push({action: entry.action, by: entry.actionBy, date: new Date(entry.timestamp)})
                })
                setHistoricalData(data.reverse())
            }
        ).catch(err => {
            console.log(err)
        });
    }, [device.id, startDate, endDate, user]);


    //update historicalData with new report
    useEffect(() => {
        console.log("GateReport useEffect");
        let data = []
        if(report.Action !== undefined || report.ActionBy !== undefined || report.Timestamp !== undefined){
            data.push({action: report.Action, by: report.ActionBy, date: new Date(report.Timestamp)})
        }
        historicalData.forEach((entry) => {
            data.push(entry)
        })
        setHistoricalData(data)
    }, [report]);

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