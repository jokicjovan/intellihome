import {useEffect, useState} from "react";
import dayjs from "dayjs";
import axios from "axios";
import {environment} from "../../../../utils/Environment.ts";
import SmartDeviceReportAction from "../Shared/SmartDeviceReportAction.tsx";
import SmartDeviceType from "../../../../models/enums/SmartDeviceType.ts";

const SolarPanelReport = ({solarPanelSystem}) => {
    const [startDate, setStartDate] = useState(dayjs().subtract(24, "hour"));
    const [endDate, setEndDate] = useState(dayjs());
    const [user, setUser] = useState("");
    const [historicalData, setHistoricalData] = useState([]);

    useEffect(() => {
        axios.get(environment + `/api/${SmartDeviceType[solarPanelSystem.type]}/GetActionHistoricalData?Id=${solarPanelSystem.id}&From=${startDate.toISOString()}&To=${endDate.toISOString()}`).then(res => {
                let data = []
                res.data.forEach((entry) => {
                    data.push({action: entry.action, by: entry.actionBy, date: new Date(entry.timestamp)})
                })
                setHistoricalData(data.reverse())
            }
        ).catch(err => {
            console.log(err)
        });
    }, [solarPanelSystem.id, startDate, endDate, user]);

    useEffect(() => {

    }, [solarPanelSystem])

    return <SmartDeviceReportAction
            inputData={historicalData}
            setParentStartDate={setStartDate}
            setParentEndDate={setEndDate}
            setParentUser={setUser}
        />
}

export default SolarPanelReport