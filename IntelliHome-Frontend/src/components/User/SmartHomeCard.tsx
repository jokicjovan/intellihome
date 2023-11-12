import { Box, Button, Container, Typography } from '@mui/material';
import { Apartment, Devices, LocationOn, PhotoSizeSelectSmall } from "@mui/icons-material";
import {environment} from "../../security/Environment.tsx";

const SmartHomeCard = (props) => {
    const isConditionMet = false; // Replace with your actual condition
    const textStyle = { fontStyle: "bold", fontWeight: "600", color: "black", margin: "5px" };
    const iconStyle = { fontStyle: "bold", fontWeight: "600", color: "#343F71", margin: "5px" };
    const containerStyle = { display: "flex", flexDirection: "row", padding: "0 10px", alignItems: "center" };
    const buttonStyle = { width: "200px",backgroundColor: "#DBDDEB", color: "#343F71", fontWeight: "600", textTransform: "none", margin: "10px auto", display: "block", borderRadius:"5px", boxShadow: "0 2px 5px rgba(0, 0, 0, 0.2)"}

    const data = props.data

    return (
        <Box sx={{ width: "280px", height: "280px", backgroundColor: isConditionMet ? "white" : "#E1E1E0", borderRadius: "15px", boxShadow: "0 2px 5px rgba(0, 0, 0, 0.2)", textAlign: "center", mb:"10px" }}>
            <Container disableGutters sx={{ display: "flex", flexDirection: "row", marginY: "auto", marginX:"5px 0" }}>
                <img
                    src={environment + '/' + data.image}
                    alt="Smart Home Image"
                    style={{ width: "50px", height: "50px", border: "5px solid #343F71", borderRadius: "8px", margin: "10px" }}
                />
                <Container disableGutters sx={{ display: "flex", flexDirection: "column", justifyContent: "center", alignItems:"flex-start" }}>
                    <Typography sx={{ fontSize: "20px", fontWeight: "600", color: "black" }}>{data.name}</Typography>
                    <Typography sx={{ fontSize: "15px", fontWeight: "600", color: "#FBC40E" }}>{data.city.name}, {data.city.country}</Typography>
                </Container>
            </Container>
            <Container disableGutters sx={containerStyle}>
                <LocationOn sx={iconStyle} /><Typography sx={textStyle}>{data.address}</Typography>
            </Container>
            <Container disableGutters sx={containerStyle}>
                <Apartment sx={iconStyle} /><Typography sx={textStyle}>{data.type == "0" ? "House" : "Flat"}</Typography>
            </Container>
            <Container disableGutters sx={containerStyle}>
                <PhotoSizeSelectSmall sx={iconStyle} /><Typography sx={textStyle}>{data.area}m<sup>2</sup></Typography>
            </Container>
            <Container disableGutters sx={containerStyle}>
                <Devices sx={iconStyle} /><Typography sx={textStyle}>0 device active</Typography>
            </Container>
            <Button sx={buttonStyle}>View details</Button>
        </Box>
    );
};

export default SmartHomeCard;
