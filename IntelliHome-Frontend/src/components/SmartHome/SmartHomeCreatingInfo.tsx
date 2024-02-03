import {Box, MenuItem, Select, TextField} from "@mui/material";
import React, {useEffect, useState} from "react";

const SmartHomeCreatingInfo = ({mapData, onMapDataChange}) => {
    const [name, setName] = useState(mapData.name || "");
    const [address, setAddress] = useState(mapData.address || "");
    const [city, setCity] = useState(mapData.city || "");
    const [country, setCountry] = useState(mapData.country || "");
    const [area, setArea] = useState(mapData.area || "");
    const [floors, setFloors] = useState(mapData.numberOfFloors || "");
    const [type, setType] = useState(mapData.type || "house");

    const styled = {
        "& label.Mui-focused": {
            color: "#FBC40E",
        },
        "& .MuiOutlinedInput-root": {
            "&.Mui-focused fieldset": {
                borderColor: "#FBC40E",
                borderRadius: "10px",
            },
            "& fieldset": {
                borderColor: "#FBC40E", // Color for non-focused state
                borderRadius: "10px",
            },
            "&:disabled": {
                borderColor: "#FBC40E", // Color for disabled state
                color: "#FBC40E", // Text color for disabled state
            },
            borderRadius: "10px",
        },
        margin: "15px auto",
        width: "45%",
        backgroundColor: "white",
        borderRadius: "10px",
        color: "#FBC40E",
    };


    const handleTypeChange = (event) => {
        setType(event.target.value);
    };

    useEffect(() => {
        onMapDataChange(
            {
                name: name,
                address: address,
                city: city,
                country: country,
                area: area,
                numberOfFloors: floors,
                type: type,
                latitude: mapData.latitude,
                longitude: mapData.longitude
            })
    }, [name, address, city, country, area, floors, type]);

    return (
        <div>
            <Box sx={{ height: '100%', width: '100%' }}>
                <Box sx={{ display: "flex", flexDirection: "row", justifyContent: "space-between" }}>
                    <TextField id="name" placeholder="Name" sx={styled} value={name} onChange={(e) => {
                        setName(e.target.value);
                    }} />
                    <TextField id="address" placeholder="Address" sx={styled} value={address} onChange={(e) => {
                        setAddress(e.target.value);
                    }} />
                </Box>
                <Box sx={{ display: "flex", flexDirection: "row" }}>
                    <TextField id="city" placeholder="City" disabled={true} sx={styled} value={city} onChange={(e) => {
                        setCity(e.target.value);
                    }} />
                    <TextField id="country" placeholder="Country" disabled={true} sx={styled} value={country} onChange={(e) => {
                        setCountry(e.target.value);
                    }} />
                </Box>
                <Box sx={{ display: "flex", flexDirection: "row" }}>
                    <TextField id="area" placeholder="Area" sx={styled} type={"number"} value={area} onChange={(e) => {
                        setArea(e.target.value);
                    }} />
                    <TextField id="floors" placeholder="Floors" sx={styled} type={"number"} value={floors} onChange={(e) => {
                        setFloors(e.target.value);
                    }} />
                </Box>
                <Box sx={{ display: "flex", flexDirection: "row", alignItems: "center" }}>
                    <Select
                        value={type}
                        onChange={handleTypeChange}
                        sx={styled}
                    >
                        <MenuItem value="flat">Flat</MenuItem>
                        <MenuItem value="house">House</MenuItem>
                    </Select>
                </Box>
            </Box>
        </div>
    );
}

export default SmartHomeCreatingInfo;
