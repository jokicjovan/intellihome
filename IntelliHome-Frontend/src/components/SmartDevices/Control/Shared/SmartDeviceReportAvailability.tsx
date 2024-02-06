import SmartDeviceAvailabilityPie from "./SmartDeviceAvailabilityPie.tsx";
import React, { useEffect, useState } from "react";
import { Box, ListItemText, MenuItem, Select, SelectChangeEvent } from "@mui/material";
import axios from "axios";
import {environment} from "../../../../utils/Environment.ts";
import SmartDeviceAvailabilityBar from "./SmartDeviceAvailabilityBar.tsx";

const SmartDeviceReportAvailability = ({ deviceId }) => {
    const [timeRange, setTimeRange] = useState<string>("6h");
    const ranges = ["1h", "2h", "3h", "6h", "1d", "7d", "30d"];
    const [onlinePercentage, setOnlinePercentage] = useState(0)
    const [offlinePercentage, setOfflinePercentage] = useState(0)
    const [data, setData] = useState([])
    const MenuProps = {
        PaperProps: {
            style: {
                maxHeight: 48 * 4.5 + 8,
                width: 250,
            },
        },
    };

    useEffect(() => {
        axios.get(environment + `/api/SmartDevice/GetAvailabilityData?id=${deviceId}&h=${timeRange}`).then(res => {
            let resdata = res.data;
            // remove null from data
            resdata = resdata.filter(function (el) {
                return el != null;
            });
            let count = resdata.length;
            let online = 0;
            let offline = 0;
            resdata.forEach((entry) => {
                online += entry.percentage;
                offline += 100 - entry.percentage;
            })
            setOnlinePercentage(online / count)
            setOfflinePercentage(offline / count)
            setData(resdata)
        }).catch(err => {
            console.log(err)
        });
    }, [timeRange]);


    const handleChangeSelect = (event: SelectChangeEvent<typeof timeRange>) => {
        setTimeRange(event.target.value);
    };

    return (
        <>
            <Box mx={3} display="flex" flexDirection="column" alignItems="center">
                <Select
                    sx={{ width: "350px", alignItems: "center" }}
                    value={timeRange}
                    placeholder="Time Range"
                    onChange={(e) => setTimeRange(e.target.value)}
                    MenuProps={MenuProps}
                >
                    {ranges.map((name: string) => (
                        <MenuItem key={name} value={name}>
                            <ListItemText primary={name} />
                        </MenuItem>
                    ))}
                </Select>
                <Box mx={3} my={10} display="flex" alignItems="center" sx={{width: "100%"}}>
                    <SmartDeviceAvailabilityPie online={onlinePercentage} offline={offlinePercentage} width={520} height={400}  />
                    <SmartDeviceAvailabilityBar data={data}  />
                </Box>
            </Box>
        </>
    );
};

export default SmartDeviceReportAvailability;
