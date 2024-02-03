import {useEffect, useState} from "react";
import axios from "axios";
import {environment} from "../../../../utils/Environment.ts";
import SmartDeviceType from "../../../../models/enums/SmartDeviceType.ts";
import SmartDeviceReportValues from "../Shared/SmartDeviceReportValues.tsx";
import SmartDeviceReportAction from "../Shared/SmartDeviceReportAction.tsx";

const SprinklerReport = ({device}) => {
    const [sprinkler, setSprinkler] = useState(device);
    const [startDate, setStartDate] = useState(new Date());
    const [endDate, setEndDate] = useState(new Date());
    const [user, setUser] = useState("");
    const [historicalData, setHistoricalData] = useState([]);

    useEffect(() => {
        axios.get(environment + `/api/${SmartDeviceType[sprinkler.type]}/GetActionHistoricalData?Id=${sprinkler.id}&From=${startDate.toISOString()}&To=${endDate.toISOString()}`).then(res => {
                let data = []
                res.data.forEach((entry) => {
                    data.push({action: entry.action, by: entry.actionBy, date: new Date(entry.timestamp)})
                })
                setHistoricalData(data.reverse())
            }
        ).catch(err => {
            console.log(err)
        });
    }, [sprinkler.id, startDate, endDate]);


    return <SmartDeviceReportAction
        inputData={historicalData}
        setParentStartDate={setStartDate}
        setParentEndDate={setEndDate}
        setParentUser={setUser}
    />
}

export default SprinklerReport