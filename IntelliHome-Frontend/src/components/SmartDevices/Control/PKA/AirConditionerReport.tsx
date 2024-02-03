import AmbientSensorReport from "./AmbientSensorReport.tsx";
import {useEffect, useState} from "react";
import dayjs from "dayjs";
import axios from "axios";
import {environment} from "../../../../utils/Environment.ts";
import SmartDeviceType from "../../../../models/enums/SmartDeviceType";
import SmartDeviceReportAction from "../Shared/SmartDeviceReportAction";

const AirConditionerReport = ({airConditioner}) => {
    const [startDate, setStartDate] = useState(dayjs().subtract(24, "hour"));
    const [endDate, setEndDate] = useState(dayjs());
    const [user, setUser] = useState("");
    const [historicalData, setHistoricalData] = useState([]);

    useEffect(() => {
        axios.get(environment + `/api/${SmartDeviceType[airConditioner.type]}/GetActionHistoricalData?Id=${airConditioner.id}&From=${startDate.toISOString()}&To=${endDate.toISOString()}`).then(res => {
                let data = []
                res.data.forEach((entry) => {
                    data.push({action: entry.action, by: entry.actionBy, date: new Date(entry.timestamp)})
                })
                setHistoricalData(data.reverse())
            }
        ).catch(err => {
            console.log(err)
        });
    }, [airConditioner.id, startDate, endDate, user]);

    useEffect(() => {

    }, [airConditioner])

    return <SmartDeviceReportAction
        inputData={historicalData}
        setParentStartDate={setStartDate}
        setParentEndDate={setEndDate}
        setParentUser={setUser}
    />
}

export default AirConditionerReport