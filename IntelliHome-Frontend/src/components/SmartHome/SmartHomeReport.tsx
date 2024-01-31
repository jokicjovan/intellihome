import SignalRSmartHomeService from "../../services/smartDevices/SignalRSmartHomeService.ts";
import React, {useEffect, useState} from "react";
import SmartDeviceReportValues from "../SmartDevices/Control/Shared/SmartDeviceReportValues.tsx";
import axios from "axios";
import {environment} from "../../utils/Environment.ts";
import dayjs from "dayjs";
import {Box} from "@mui/material";
import {Chart} from "react-google-charts";

const SmartHomeReport = ({smartHomeId}) => {
    const [startDate, setStartDate] = useState(dayjs().subtract(24, "hour"));
    const [endDate, setEndDate] = useState(dayjs());
    const headerRow = ["Date", "ProductionPerMinute", "ConsumptionPerMinute", "GridPerMinute"];
    const [historicalData, setHistoricalData] = useState([headerRow]);
    const [recentData, setRecentData] = useState([headerRow])
    const signalRSmartHomeService = new SignalRSmartHomeService();

    const smartHomeSubscriptionResultCallback = (result) => {
        console.log('HomePage subscription result:', result);
    }

    const smartHomeDataCallback = (result) => {
        result = JSON.parse(result);
        console.log('data result:', result);
        setRecentData((prevData: any) => {
            const filteredData = prevData.filter((item, index) => {
                    if (index !== 0) {
                        return new Date(item[0]).getHours() + 1 >= new Date().getHours();
                    }
                    return true
                }
            );
            return [...filteredData, [new Date(),
                parseFloat(result.productionPerMinute),
                parseFloat(result.consumptionPerMinute),
                parseFloat(result.gridPerMinute)]];
        });
    };

    useEffect(() : any => {
        if (smartHomeId) {
            signalRSmartHomeService.startConnection().then(() => {
                console.log('SignalR home connection established');
                signalRSmartHomeService.receiveSmartHomeSubscriptionResult(smartHomeSubscriptionResultCallback);
                signalRSmartHomeService.receiveSmartHomeData(smartHomeDataCallback);
                signalRSmartHomeService.subscribeToSmartHome(smartHomeId);
            });
        }

        return () => signalRSmartHomeService.stopConnection().then(() => {
            console.log('SignalR connection stopped');
        });
    }, [smartHomeId])

    useEffect(() => {
        fetchDataAndUpdateState(dayjs().subtract(1, "hour"), dayjs(), setRecentData);
    }, [smartHomeId]);

    useEffect(() => {
        fetchDataAndUpdateState(startDate, endDate, setHistoricalData);
    }, [smartHomeId, startDate, endDate]);

    const fetchDataAndUpdateState = (from, to, setData) => {
        axios.get(`${environment}/api/SmartHome/GetUsageHistoricalData?Id=${smartHomeId}&From=${from.toISOString()}&To=${to.toISOString()}`)
            .then(res => {
                const dataRows = res.data.map(({
                                                   productionPerMinute,
                                                   consumptionPerMinute,
                                                   gridPerMinute,
                                                   timestamp
                                               }) => [
                    new Date(timestamp),
                    parseFloat(productionPerMinute),
                    parseFloat(consumptionPerMinute),
                    parseFloat(gridPerMinute)
                ]);
                const transformedData = [headerRow, ...dataRows];
                setData(transformedData);
            })
            .catch(err => {
                console.log(err);
            });
    };

    const options = {
        hAxis: {
            title: "Time",
        },
        vAxis: {
            title: "KWh",
        },
        title: "Recent Consumption, Production, Grid (taken/given)"
    };

    return <Box mt={1} display="flex" flexDirection={"column"} overflow="auto">
        <Box display="flex" justifyContent="center">
            <Box width="80%">
                <Chart
                    chartType="LineChart"
                    width="100%"
                    height="300px"
                    data={recentData}
                    options={options}
                />
            </Box>
        </Box>
        <Box>
            <SmartDeviceReportValues
                inputData={historicalData}
                setParentStartDate={setStartDate}
                setParentEndDate={setEndDate}
                xLabel={options.hAxis.title}
                yLabel={options.vAxis.title}
                title="Historical Consumption, Production, Grid (taken/given)"
            />
        </Box>
    </Box>
}

export default SmartHomeReport