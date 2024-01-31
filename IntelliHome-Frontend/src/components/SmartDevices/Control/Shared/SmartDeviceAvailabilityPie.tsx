import * as React from 'react';
import { PieChart } from '@mui/x-charts/PieChart';
import {useEffect, useState} from "react";

const SmartDeviceAvailabilityPie = ({width, height, online, offline}) => {
    const [onlinePercentage, setOnlinePercentage] = useState(online)
    const [offlinePercentage, setOfflinePercentage] = useState(offline)

    useEffect(() => {
        setOnlinePercentage(online)
        setOfflinePercentage(offline)
    }, [online, offline])

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