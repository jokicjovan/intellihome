import { Box, Button, Container, Typography } from '@mui/material';
import { Apartment, Devices, LocationOn, PhotoSizeSelectSmall } from "@mui/icons-material";
import {useNavigate} from "react-router-dom";
import {environment} from "../../utils/Environment.ts";

const SmartHomeCard = (props) => {
    const data = props.data
    const navigate = useNavigate();
    const isConditionMet = data.isApproved; // Replace with your actual condition
    const textStyle = { fontStyle: "bold", fontWeight: "600", color: "black", margin: "5px" };
    const iconStyle = { fontStyle: "bold", fontWeight: "600", color: "#343F71", margin: "5px" };
    const containerStyle = { display: "flex", flexDirection: "row", padding: "0 10px", alignItems: "center" };
    const buttonStyle = {
        width: "200px",
        backgroundColor: "#DBDDEB",
        color: "#343F71",
        fontWeight: "600",
        textTransform: "none",
        margin: "10px auto",
        display: "block",
        borderRadius: "5px",
        boxShadow: "0 2px 5px rgba(0, 0, 0, 0.2)"
    }
    const boxStyle = {
        width: "18vw",
        height: "30vh",
        backgroundColor: isConditionMet ? "white" : "#D0D0D0",
        borderRadius: "15px",
        textAlign: "center",
        mb: "10px",
        display: "flex",
        flexDirection: "column",
    }

    const handleButtonClick = () => {
        if (isConditionMet) navigate("/smartHome/" + data.id)
    }


    return (
        <Box sx={boxStyle}>
            <Container disableGutters sx={{ display: "flex", flexDirection: "row", marginY: "0vh", marginX: "5px 0" }}>
                <img
                    src={environment + '/' + data.image}
                    alt="Smart HomePage Image"
                    style={{ width: "60px", height: "60px", minWidth:"60px", maxWidth:"60px", minHeight:"60px", maxHeight:"60px", border: "5px solid #343F71", borderRadius: "8px", margin: "10px" }}
                />
                <Container disableGutters sx={{ display: "flex", flexDirection: "column", justifyContent: "center", alignItems: "flex-start" }}>
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
            <Button sx={{ ...buttonStyle, mt: 3 }} onClick={handleButtonClick}>View details</Button>
        </Box>
    );
};

export default SmartHomeCard;
