import {useEffect, useState} from "react";

const SprinklerReport = ({device}) => {
    const [sprinkler, setSprinkler] = useState(device);
    const [startDate, setStartDate] = useState(new Date());
    const [endDate, setEndDate] = useState(new Date());
    const [historicalData, setHistoricalData] = useState([]);

    useEffect(() => {
        if (sprinkler!==undefined) {
            //axios get
        }
    }, [sprinkler.id, startDate, endDate]);


    return <>
        {/*<SmartDeviceReportValues*/}
        {/*    setParentStartDate={setStartDate}*/}
        {/*    setParentEndDate={setEndDate}*/}
        {/*    xLabel={"time"}*/}
        {/*    yLabel={"Temperature & Humidity"}*/}
        {/*    inputData={historicalData}*/}
        {/*    title={"Ambient Sensor Data"}*/}
        {/*/>*/}
    </>
}

export default SprinklerReport