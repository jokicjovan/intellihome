import {Box, Container, CssBaseline, Typography} from "@mui/material";
import {useState} from "react";
import {LineChart} from "@mui/x-charts";

const AmbientSensorControl = () => {
    const [temperature,setTemperature]=useState(25)
    const [humidity,setHumidity]=useState(86)
    return <Box display="flex" flexDirection="column"><Box m={3} display="flex" flexDirection="row">
        <Box mx={3} display="flex" justifyContent="center" flexDirection="column" alignItems="center" bgcolor="white" width="350px" height="350px" borderRadius="25px">
            <Typography fontSize="30px" fontWeight="600"> TEMPERATURE</Typography>
            <Typography fontSize="100px" fontWeight="800">{temperature}°C</Typography>
        </Box>
        <Box mx={3} display="flex" justifyContent="center" flexDirection="column" alignItems="center" bgcolor="white" width="350px" height="350px" borderRadius="25px">
            <Typography fontSize="30px" fontWeight="600"> HUMIDITY</Typography>
            <Typography fontSize="100px" fontWeight="800">{humidity}%</Typography>
        </Box>

        <Box mx={3} display="flex" justifyContent="center" flexDirection="column" alignItems="center" bgcolor="white" width="700px" height="350px" borderRadius="25px">
            <LineChart
                xAxis={[{ data: [1, 2, 3, 5, 8, 10] }]}
                series={[
                    {
                        data: [2, 5.5, 2, 8.5, 1.5, 5],
                        area: true,
                        showMark:false,
                        color:"rgb(52,63,113)"

                    },
                ]}
                sx={{
                    '& .MuiAreaElement-series-Germany': {
                    fill: "rgba(52,63,113,0.3)",
                },
                }}
                width={600}
                height={300}
            />
        </Box>
    </Box></Box>
}

export default AmbientSensorControl;