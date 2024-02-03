import {
    Box, Checkbox,
    Chip,
    ListItemText, MenuItem, Select,
    SelectChangeEvent,
    TablePagination,
    Typography
} from "@mui/material";
import format from 'date-fns/format'
import React, {useEffect, useState} from "react";
import {AdapterDayjs} from "@mui/x-date-pickers/AdapterDayjs";
import {LocalizationProvider, DateTimePicker} from "@mui/x-date-pickers";
import dayjs from "dayjs";
/*
    USAGE
    input data must be in this format
    [
        {action: "some action", by: "Vukasin", date: new Date()},
        {action: "some action", by: "Marko", date: new Date()}
    ]

 */

const SmartDeviceReportAction = ({inputData, setParentStartDate, setParentEndDate, setParentUser}) => {
    // @ts-ignore
    type TDate = TDate | null;
    const [page, setPage] = useState(0);
    const [filteredData, setFilteredData] = useState(inputData);
    const [admins, setAdmins] = useState(Array.from(new Set(filteredData.map(obj => obj.by))));
    const [pagePerRow, setPagePerRow] = useState(10);
    const [personName, setPersonName] = React.useState<string[]>([]);
    const [startDate, setStartDate] = useState<TDate>(dayjs().subtract(24, "hour"));
    const [endDate, setEndDate] = useState<TDate>(dayjs());
    useEffect(() => {
        setPage(0);
        // @ts-ignore
        if (personName == "" || personName == [])
            setFilteredData(inputData.filter(item => dayjs(item.date.toString()).isAfter(new Date(startDate.toString())) && dayjs(item.date.toString()).isBefore(new Date(endDate.toString()))))
        else
            setFilteredData(inputData.filter(item => dayjs(item.date.toString()).isAfter(new Date(startDate.toString())) && dayjs(item.date.toString()).isBefore(new Date(endDate.toString())) && personName.includes(item.by)))
        setParentStartDate(startDate)
        setParentEndDate(endDate)
        setParentUser(personName)
        setAdmins(Array.from(new Set(inputData.map(obj => obj.by))));
    }, [startDate, endDate, personName]);

    useEffect(() => {
        // @ts-ignore
        if (personName == "" || personName == [])
            setFilteredData(inputData);
        else
            setFilteredData(inputData.filter(item => personName.includes(item.by)))
        // setFilteredData(inputData);
        setAdmins(Array.from(new Set(inputData.map(obj => obj.by))));
    }, [inputData]);

    const handleChangeSelect = (event: SelectChangeEvent<typeof personName>) => {
        const {
            target: {value},
        } = event;
        setPersonName(
            typeof value === 'string' ? value.split(',') : value,
        );
    };
    const handleChangePage = (
        event: React.MouseEvent<HTMLButtonElement> | null,
        newPage: number,
    ) => {
        setPage(newPage);
    };
    const handleChangeRowsPerPage = (
        event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
    ) => {
        setPagePerRow(parseInt(event.target.value, 10));
        setPage(0);
    };
    const MenuProps = {
        PaperProps: {
            style: {
                maxHeight: 48 * 4.5 + 8,
                width: 250,
            },
        },
    };

    return <Box mx={3} display="flex" flexDirection="column" alignItems="center">
        <Box mt={3}  width="80%" display="flex" flexDirection="row" justifyContent="center" alignItems="center">
            <LocalizationProvider dateAdapter={AdapterDayjs}>
                <DateTimePicker
                    ampm={false}
                    label="Start Date"
                    value={startDate}
                    sx={{marginRight: "10px"}}
                    onChange={(newValue) => setStartDate(newValue)}
                />
                <Typography mx={2} fontSize="24px">-</Typography>
                <DateTimePicker
                    ampm={false}
                    label="End Date"
                    sx={{marginLeft: "10px"}}
                    value={endDate}
                    onChange={(newValue) => setEndDate(newValue)}
                />
                <Typography mx={2} fontSize="18px">by</Typography>
                <Select
                    sx={{width: "350px", alignItems:"center"}}
                    multiple
                    value={personName}
                    placeholder="Action by"
                    onChange={handleChangeSelect}
                    renderValue={(selected) => selected.join(', ')}
                    MenuProps={MenuProps}
                >
                    {admins.map((name : string) => (
                        <MenuItem key={name} value={name}>
                            <Checkbox checked={personName.indexOf(name) > -1}/>
                            <ListItemText primary={name}/>
                        </MenuItem>
                    ))}
                </Select>
            </LocalizationProvider>
            </Box>
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

        <Box bgcolor="white" p={2} borderRadius="25px" width="100%" mt={2}>
            <div style={{display: "flex", flexDirection: "column", minHeight: "300px", width: "100%"}}>
                <Box width="98%" margin="0 auto" height={"2px"} bgcolor="rgba(0, 0, 0, 0.20)"/>
                {filteredData.slice(page * pagePerRow, (page + 1) * pagePerRow).map(item => <Box key={item.action+item.by+item.date.toString()}>
                    <Box mb={1}  width="100%" mt={1} display="grid"  gridTemplateColumns="1fr 1fr 1fr">
                        <Box display="flex" justifyContent="center" alignItems="center" gridColumn={1}>
                            {item.action.toString()}
                        </Box>
                        <Box display="flex" justifyContent="center" alignItems="center" gridColumn={2}>
                            {format(item.date, 'MM/dd/yyyy hh:mm')}
                        </Box>
                        <Box  display="flex" justifyContent="center" alignItems="center" gridColumn={3}>
                            {item.by.toString()}
                        </Box>
                    </Box>
                    <Box width="98%" margin="0 auto" height={"2px"} bgcolor="rgba(0, 0, 0, 0.20)"/></Box>)}
                {filteredData.length==0&&<Box display="flex" flexDirection="column" justifyContent="center" alignItems="center">
                    <Typography mt={1} mb={1}>No results :(</Typography>
                    <Box width="98%" margin="0 auto" height={"2px"} bgcolor="rgba(0, 0, 0, 0.20)"/>
                </Box>}

                <TablePagination sx={{marginTop: "auto"}} className="paginator" component="div" page={page}
                                 rowsPerPage={pagePerRow}
                                 onPageChange={handleChangePage} onRowsPerPageChange={handleChangeRowsPerPage}
                                 rowsPerPageOptions={[5, 10]}
                                 count={filteredData.length}/>
            </div>
        </Box>
    </Box>
}

export default SmartDeviceReportAction;