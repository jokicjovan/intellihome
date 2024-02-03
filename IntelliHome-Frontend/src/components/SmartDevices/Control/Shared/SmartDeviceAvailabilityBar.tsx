import * as React from 'react';
import {useEffect, useState} from "react";
import {axisClasses, BarChart} from "@mui/x-charts";

const SmartDeviceAvailabilityBar = ({data}) => {
    const [dataset, setDataset] = useState([{timestamp: "00:00", online: 0, offline: 0}])
    let [unit, setUnit] = useState("h")
    useEffect(() => {
        let list = []
        if (data.length == 0) {
            return
        }
        console.log(data)
        data.forEach((entry) => {
            setUnit(entry.units)
            console.log(unit)
            if (unit == "h") {
                list.push({timestamp: entry.timestamp.split("T")[1].split(":")[0], online: entry.duration, offline: 60 - entry.duration})
            }
            else {
                list.push({timestamp: entry.timestamp.split("T")[0].split(":")[0], online: entry.duration, offline: 24- entry.duration})
            }

        })
        setDataset(list)
        console.log(data)
    }, [data])


    const chartSetting = {
        yAxis: [
            {
                label: `Time`,
            },
        ],
        width: 500,
        height: 300,
        sx: {
            [`.${axisClasses.left} .${axisClasses.label}`]: {
                transform: 'translate(-20px, 0)',
            },
        },
    };

    const valueFormatter = (value: number) => `${value}` + (unit == "h"? "m": "h");


    return (
        <BarChart
            dataset={dataset}
            xAxis={[{ scaleType: 'band', dataKey: 'timestamp' }]}
            series={[
                { dataKey: 'online', label: 'Online', valueFormatter },
                { dataKey: 'offline', label: 'Offline', valueFormatter },

            ]}
            {...chartSetting}
        />
    );
}

export default SmartDeviceAvailabilityBar;