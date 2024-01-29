import * as React from 'react';
import { PieChart } from '@mui/x-charts/PieChart';
import {useEffect, useState} from "react";
import axios from "axios";
import {environment} from "../../../../utils/Environment.ts";

const SmartDeviceAvailabilityPie = ({width, height, deviceId, h}) => {
    const [id, setId] = useState(deviceId)
    const [onlinePercentage, setOnlinePercentage] = useState(0)
    const [offlinePercentage, setOfflinePercentage] = useState(0)
    const [range, setRange] = useState(h)

    useEffect(() => {
        axios.get(environment + `/api/SmartDevice/GetAvailabilityData?id=${id}&h=${range}`).then(res => {
            let count = res.data.length;
            let online = 0;
            let offline = 0;
            res.data.forEach((entry) => {
                online += entry.percentage;
                offline += 100 - entry.percentage;
            })
            setOnlinePercentage(online / count)
            setOfflinePercentage(offline / count)

        }).catch(err => {
            console.log(err)
        });
    }, []);

    return (
        <PieChart
            series={[
                {
                    data: [
                        { value: onlinePercentage, label: 'ONLINE' },
                        { value: offlinePercentage, label: 'OFFLINE' },
                    ],
                },
            ]}
            width={width}
            height={height}
        />
    );
}

export default SmartDeviceAvailabilityPie;