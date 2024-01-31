import {useEffect, useState} from "react";
import axios from "axios";
import {environment} from "../../../../utils/Environment.ts";
import SmartDeviceReportValues from "../Shared/SmartDeviceReportValues";

const AmbientSensorReport = ({device}) => {
    const [ambientSensor, setAmbientSensor] = useState(device);
    const [startDate, setStartDate] = useState(new Date());
    const [endDate, setEndDate] = useState(new Date());
    const [historicalData, setHistoricalData] = useState([]);

    useEffect(() => {
        if (ambientSensor!==undefined) {
            axios.get(environment + `/api/AmbientSensor/GetHistoricalData?Id=${ambientSensor.id}&From=${startDate.toISOString()}&To=${endDate.toISOString()}`).then(res => {
                    let data = [['date', 'temperature', 'humidity']]
                    res.data.forEach((entry) => {
                        data.push([new Date(entry.timestamp), entry.temperature, entry.humidity])
                    })
                    console.log(data)
                    setHistoricalData(data)
                }
            ).catch(err => {
                console.log(err)
            });
        }
    }, [ambientSensor.id, startDate, endDate]);


    return <>
        <SmartDeviceReportValues
            setParentStartDate={setStartDate}
            setParentEndDate={setEndDate}
            xLabel={"time"}
            yLabel={"Temperature & Humidity"}
            inputData={historicalData}
            title={"Ambient Sensor Data"}
        />
    </>
}

export default AmbientSensorReport