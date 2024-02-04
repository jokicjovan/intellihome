import {useNavigate} from "react-router-dom";
import {Box, Button, Container, Typography} from "@mui/material";
import {environment} from "../../utils/Environment.ts";
import {Apartment, LocationOn, PhotoSizeSelectSmall} from "@mui/icons-material";

const SmartHomeConsumptionCard = ({smartHome, onButtonClick}) => {
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
        width: "15vw",
        height: "28vh",
        backgroundColor: "white",
        borderRadius: "15px",
        textAlign: "center",
        mb: "10px",
        display: "flex",
        flexDirection: "column",
    }

    return (
        <Box sx={boxStyle}>
            <Container disableGutters sx={{ display: "flex", flexDirection: "row", marginY: "0vh", marginX: "5px 0" }}>
                <img
                    src={environment + '/' + smartHome.image}
                    alt="Smart HomePage Image"
                    style={{ width: "60px", height: "60px", minWidth:"60px", maxWidth:"60px", minHeight:"60px", maxHeight:"60px", border: "5px solid #343F71", borderRadius: "8px", margin: "10px" }}
                />
                <Container disableGutters sx={{ display: "flex", flexDirection: "column", justifyContent: "center", alignItems: "flex-start" }}>
                    <Typography sx={{ fontSize: "20px", fontWeight: "600", color: "black" }}>{smartHome.name}</Typography>
                    <Typography sx={{ fontSize: "15px", fontWeight: "600", color: "#FBC40E" }}>{smartHome.city.name}, {smartHome.city.country}</Typography>
                </Container>
            </Container>
            <Container disableGutters sx={containerStyle}>
                <LocationOn sx={iconStyle} /><Typography sx={textStyle}>{smartHome.address}</Typography>
            </Container>
            <Container disableGutters sx={containerStyle}>
                <Apartment sx={iconStyle} /><Typography sx={textStyle}>{smartHome.type == "0" ? "House" : "Flat"}</Typography>
            </Container>
            <Container disableGutters sx={containerStyle}>
                <PhotoSizeSelectSmall sx={iconStyle} /><Typography sx={textStyle}>{smartHome.area}m<sup>2</sup></Typography>
            </Container>
            <Button sx={{ ...buttonStyle, mt: 2 }} onClick={() => onButtonClick(smartHome.id)}>View Consumption</Button>
        </Box>
    );
};

export default SmartHomeConsumptionCard;
