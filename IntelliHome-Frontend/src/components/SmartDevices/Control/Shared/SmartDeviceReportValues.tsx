import {Box, Chip, Typography} from "@mui/material";
import {useEffect, useState} from "react";
import {AdapterDayjs} from "@mui/x-date-pickers/AdapterDayjs";
import {LocalizationProvider, DateTimePicker} from "@mui/x-date-pickers";
import dayjs from "dayjs";
import {Chart} from "react-google-charts";


/*
    USAGE
    input data example
    inputData=[["date", "dogs", "cats"],
        [new Date().toString()), 1, 2],
        [new Date().toString()), 2, 7]]
    IMPORTANT
    must use Date
    date must be called date!
    date must be 1st column
    after that you can set multiple columns
    ----------------
    XLabel is name of horizontal axis
    ----------------
    YLabel is name of vertical axis
    ----------------
    colorScheme is optional prop to set custom color lines
    for example you can set it like this
    colorScheme={{0: { color: '#e2431e' },
                  1: { color: '#e7711b' }}}

 */
const SmartDeviceReportValues = ({xLabel,yLabel,inputData,colorScheme={}}) => {
    type TDate = TDate | null;
    const [data, setData] = useState(inputData)
    const [hasError, setHasError] = useState(false)
    const [filteredData, setFilteredData] = useState(data)
    const options = {
        hAxis: {
            title: xLabel,
        },
        vAxis: {
            title: yLabel,
        },
        series: colorScheme
    };
    const [startDate, setStartDate] = useState<TDate>(dayjs().subtract(24, "hour"));
    const [endDate, setEndDate] = useState<TDate>(dayjs());
    useEffect(()=>{
        setData(inputData)
        setFilteredData(filterData())
    },[inputData])
    const filterData = () => {
        let header = data[0]
        const restOfData = data.slice(1)
        const filteredData = restOfData.filter((entry) => {
            const entryDate = entry[0]
            return entryDate >= new Date(startDate.toString()) && entryDate <= new Date(endDate.toString());
        })
        if (filteredData.length == 0)
            return [['', ''], [0, 0]]
        else
            return [[...header], ...filteredData]
    }

    useEffect(() => {
        if (endDate.diff(startDate, 'day') <= 30) {
            setHasError(false)
            setFilteredData(filterData())
        } else {
            setHasError(true)
            setFilteredData([['', ''], [0, 0]])
        }

    }, [startDate, endDate])
    return <Box display="flex" flexDirection="column" alignItems="center">
        <Box mt={3} width="50%" display="flex" flexDirection="row" justifyContent="center" alignItems="center">
            <LocalizationProvider dateAdapter={AdapterDayjs}>
                <DateTimePicker
                    ampm={false}
                    label="Start Date"
                    value={startDate}
                    sx={{marginRight: "10px"}}
                    onChange={(newValue) => setStartDate(newValue)}
                />
                <Typography fontSize="24px">-</Typography>
                <DateTimePicker
                    ampm={false}
                    label="End Date"
                    sx={{marginLeft: "10px"}}
                    value={endDate}
                    onChange={(newValue) => setEndDate(newValue)}
                />
            </LocalizationProvider></Box>
        {hasError && <Box display='flex' justifyContent='center' alignItems='center'>
            <Typography color='red' fontSize="22">Date range is over one month long</Typography>
        </Box>}
        <Box display="flex" mt={2} flexDirection="row">
            <Chip
                label="6 hours"
                sx={{
                    fontSize: "18px",
                    fontWeight: "500",
                    marginLeft: "5px",
                    marginRight: "5px",
                    ':hover': {backgroundColor: "#FBC40E"}
                }}
                onClick={() => {
                    let currentDate = dayjs().subtract(6, "hour")
                    setStartDate(currentDate)
                    setEndDate(dayjs())
                }}
            />
            <Chip
                label="12 hours"
                sx={{
                    fontSize: "18px",
                    fontWeight: "500",
                    marginLeft: "5px",
                    marginRight: "5px",
                    ':hover': {backgroundColor: "#FBC40E"}
                }}
                onClick={() => {
                    let currentDate = dayjs().subtract(12, "hour")
                    setStartDate(currentDate)
                    setEndDate(dayjs())
                }}
            />
            <Chip
                label="24 hours"
                sx={{
                    fontSize: "18px",
                    fontWeight: "500",
                    marginLeft: "5px",
                    marginRight: "5px",
                    ':hover': {backgroundColor: "#FBC40E"}
                }}
                onClick={() => {
                    let currentDate = dayjs().subtract(24, "hour")
                    setStartDate(currentDate)
                    setEndDate(dayjs())
                }}
            />
            <Chip
                label="1 week"
                sx={{
                    fontSize: "18px",
                    fontWeight: "500",
                    marginLeft: "5px",
                    marginRight: "5px",
                    ':hover': {backgroundColor: "#FBC40E"}
                }}
                onClick={() => {
                    let currentDate = dayjs().subtract(7, "day")
                    setStartDate(currentDate)
                    setEndDate(dayjs())
                }}
            />
            <Chip
                label="1 month"
                sx={{
                    fontSize: "18px",
                    fontWeight: "500",
                    marginLeft: "5px",
                    marginRight: "5px",
                    ':hover': {backgroundColor: "#FBC40E"}
                }}
                onClick={() => {
                    let currentDate = dayjs().subtract(30, "day")
                    setStartDate(currentDate)
                    setEndDate(dayjs())
                }}
            />
        </Box>

        <Box mx={3} mt={3} display="flex" justifyContent="center" flexDirection="column" alignItems="center"
             bgcolor="white"
             width="80%" borderRadius="25px">
            <Chart
                chartType="LineChart"
                width="100%"
                height="400px"
                data={filteredData}
                options={options}
            />
        </Box>
    </Box>
}

export default SmartDeviceReportValues;