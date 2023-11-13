import { Box, TextField } from "@mui/material";
import React, { useState } from "react";

const SmartHomeCreatingInfo = ({mapData}) => {
    const [name, setName] = useState("");
    const [address, setAddress] = useState(mapData.address || "");
    const [city, setCity] = useState(mapData.city || "");
    const [country, setCountry] = useState(mapData.country || "");
    const [area, setArea] = useState("");
    const [floors, setFloors] = useState("");

    const styled = {
        "& label.Mui-focused": {
            color: "#FBC40E"
        },
        "& .MuiOutlinedInput-root": {
            "&.Mui-focused fieldset": {
                borderColor: "#FBC40E",
                borderRadius: "10px"
            },
            borderRadius: "10px"
        },
        margin: "15px auto", width: "45%", backgroundColor: "white", borderRadius: "10px"
    };

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
                    <TextField id="city" placeholder="City" sx={styled} value={city} onChange={(e) => {
                        setCity(e.target.value);
                    }} />
                    <TextField id="country" placeholder="Country" sx={styled} value={country} onChange={(e) => {
                        setCountry(e.target.value);
                    }} />
                </Box>
                <Box sx={{ display: "flex", flexDirection: "row" }}>
                    <TextField id="area" placeholder="Area" sx={styled} value={area} onChange={(e) => {
                        setArea(e.target.value);
                    }} />
                    <TextField id="floors" placeholder="Floors" sx={styled} value={floors} onChange={(e) => {
                        setFloors(e.target.value);
                    }} />
                </Box>
            </Box>
        </div>
    );
}

export default SmartHomeCreatingInfo;
