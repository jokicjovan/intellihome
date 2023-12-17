import {Box, Container, CssBaseline, Typography} from "@mui/material";
import React, {useEffect, useState} from "react";
import {Chart} from "react-google-charts";
import axios from "axios";
import {environment} from "../../../../security/Environment";
import SmartDeviceType from "../../../../models/enums/SmartDeviceType";

const AmbientSensorControl = ({smartDevice}) => {
    const [temperature, setTemperature] = useState(smartDevice.temperature)
    const [humidity, setHumidity] = useState(smartDevice.humidity)
    const [data, setData] = useState([["Date", "Temperature", "Humidity"], [0, 0, 0]])
    const options = {
        hAxis: {
            title: "Time",
        },
        vAxis: {
            title: "Temperature Humidity",
        }
    };

    useEffect(() => {
        setTemperature(smartDevice.temperature)
        setHumidity(smartDevice.humidity)

        setData((prevData) => {

            const filteredData = prevData.filter((item, index) => {
                    if (index !== 0) {
                        if (new Date(item[0]).getHours()+1>= new Date().getHours()) return true
                        else return false
                    }
                    else return true
                }
            );
            console.log(filteredData)
            return [...filteredData, [new Date(), smartDevice.temperature, smartDevice.humidity]];
        });
    }, [smartDevice])
    const getHistoricalData = async () => {
        if (Object.keys(smartDevice).length === 0) {
            return;
        }
        const startDate = new Date();
        startDate.setHours(startDate.getHours() - 1)
        const endDate = new Date();
        console.log(startDate)
        console.log(endDate)
        axios.get(
            environment +
            `/api/${SmartDeviceType[smartDevice.type]}/GetHistoricalData?Id=${smartDevice.id}&from=${startDate.toISOString()}&to=${endDate.toISOString()}`
        ).then((res) => {
            console.log(res)
            const transformedData = res.data.length > 0 ? [
                ["Date", "Temperature", "Humidity"],
                ...res.data.map((o) => [
                    new Date(o.timestamp),
                    o.temperature,
                    o.humidity
                ]),
            ] : [["Date", "Temperature", "Humidity"], [0, 0, 0]];

            setData(transformedData);
        }).catch((err) => {
            console.log(err)
        })
    }


    useEffect(() => {
        getHistoricalData();
    }, [smartDevice.id])

    return <Box display="flex" flexDirection="column"><Box m={3} display="flex" flexDirection="row">
        <Box mx={3} display="flex" justifyContent="center" flexDirection="column" alignItems="center" bgcolor="white"
             width="350px" height="350px" borderRadius="25px">
            <Typography fontSize="30px" fontWeight="600"> TEMPERATURE</Typography>
            <Typography fontSize="100px" fontWeight="800">{temperature}°C</Typography>
        </Box>
        <Box mx={3} display="flex" justifyContent="center" flexDirection="column" alignItems="center" bgcolor="white"
             width="350px" height="350px" borderRadius="25px">
            <Typography fontSize="30px" fontWeight="600"> HUMIDITY</Typography>
            <Typography fontSize="100px" fontWeight="800">{humidity}%</Typography>
        </Box>

        <Box mx={3} display="flex" justifyContent="center" flexDirection="column" alignItems="center" bgcolor="white"
             width="600px" height="350px" borderRadius="25px">
            <Chart
                chartType="LineChart"
                width="100%"
                height="300px"
                data={data}
                options={options}
            />
        </Box>
    </Box></Box>
}

export default AmbientSensorControl;