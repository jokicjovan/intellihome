import React, {useEffect, useState} from "react";
import dayjs from "dayjs";
import axios from "axios";
import {environment} from "../../utils/Environment.ts";
import {Box} from "@mui/material";
import {Chart} from "react-google-charts";
import SmartDeviceReportValues from "../SmartDevices/Control/Shared/SmartDeviceReportValues.tsx";

const CityConsumptionReport = ({cityId}) => {
    const [startDate, setStartDate] = useState(dayjs().subtract(24, "hour"));
    const [endDate, setEndDate] = useState(dayjs());
    const headerRow = ["Date", "ProductionPerMinute", "ConsumptionPerMinute", "GridPerMinute"];
    const [historicalData, setHistoricalData] = useState([headerRow]);
    const [recentData, setRecentData] = useState([headerRow])

    useEffect(() => {
        fetchDataAndUpdateState(dayjs().subtract(1, "hour"), dayjs(), setRecentData);
    }, [cityId]);

    useEffect(() => {
        fetchDataAndUpdateState(startDate, endDate, setHistoricalData);
    }, [cityId, startDate, endDate]);

    const fetchDataAndUpdateState = (from, to, setData) => {
        axios.get(`${environment}/api/City/GetCityHistoricalData?Id=${cityId}&From=${from.toISOString()}&To=${to.toISOString()}`)
            .then(res => {
                console.log(res.data)
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

export default CityConsumptionReport