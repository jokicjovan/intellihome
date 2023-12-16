import {useEffect, useState} from "react";
import axios from "axios";
import {environment} from "../../../../utils/Environment.ts";
import SmartDeviceReportValues from "../Shared/SmartDeviceReportValues.tsx";

const LampReport = ({device}) => {
    const [lamp, setLamp] = useState(device);
    const [startDate, setStartDate] = useState(new Date());
    const [endDate, setEndDate] = useState(new Date());
    const [historicalData, setHistoricalData] = useState([]);

    useEffect(() => {
        axios.get(environment + `/api/Lamp/GetHistoricalData?Id=${lamp.id}&From=${startDate.toISOString()}&To=${endDate.toISOString()}`).then(res => {
                // setHistoricalData(res.data)
                let data = [['date', 'brightness']]
                res.data.forEach((entry) => {
                    data.push([new Date(entry.timestamp), entry.currentBrightness])
                })
                console.log(data)
                setHistoricalData(data)
            }
        ).catch(err => {
            console.log(err)
        });
    }, [lamp.id, startDate, endDate]);


    return <>
        <SmartDeviceReportValues
            setParentStartDate={setStartDate}
            setParentEndDate={setEndDate}
            xLabel={"time"}
            yLabel={"brightness"}
            inputData={historicalData}
            title={"Historical brightness data"}
        />
    </>
}

export default LampReport