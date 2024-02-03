import {useEffect, useState} from "react";
import SmartDeviceReportAction from "../Shared/SmartDeviceReportAction";
import dayjs from "dayjs";
import axios from "axios";
import {environment} from "../../../../utils/Environment.ts";
import SmartDeviceType from "../../../../models/enums/SmartDeviceType";

const WashingMachineReport = ({device}) => {
    const [startDate, setStartDate] = useState(dayjs().subtract(24, "hour"));
    const [endDate, setEndDate] = useState(dayjs());
    const [user, setUser] = useState("");
    const [historicalData, setHistoricalData] = useState([]);

    useEffect(() => {
        axios.get(environment + `/api/${SmartDeviceType[device.type]}/GetActionHistoricalData?Id=${device.id}&From=${startDate.toISOString()}&To=${endDate.toISOString()}`).then(res => {
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

    useEffect(() => {

    }, [device])


    return <SmartDeviceReportAction
        inputData={historicalData}
        setParentStartDate={setStartDate}
        setParentEndDate={setEndDate}
        setParentUser={setUser}
    />
}

export default WashingMachineReport