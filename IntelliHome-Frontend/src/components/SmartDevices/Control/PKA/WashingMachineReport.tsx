import {useEffect, useState} from "react";

const WashingMachineReport = ({device}) => {
    const [washingMachine, setWashingMachine] = useState(device);
    const [startDate, setStartDate] = useState(new Date());
    const [endDate, setEndDate] = useState(new Date());
    const [historicalData, setHistoricalData] = useState([]);

    useEffect(() => {
        if (washingMachine!==undefined) {
            //axios get
        }
    }, [washingMachine.id, startDate, endDate]);


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

export default WashingMachineReport