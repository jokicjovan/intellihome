import {Box, Button, Container, Typography} from "@mui/material";

const CityConsumptionCard = ({city, onButtonClick}) => {
    const buttonStyle = {
        width: "200px",
        backgroundColor: "#DBDDEB",
        color: "#343F71",
        fontWeight: "600",
        textTransform: "none",
        display: "block",
        borderRadius: "5px",
        boxShadow: "0 2px 5px rgba(0, 0, 0, 0.2)"
    }
    const boxStyle = {
        width: "300px",
        height: "200px",
        backgroundColor: "white",
        borderRadius: "15px",
        textAlign: "center",
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        verticalAlign: "center"
    }

    return (
        <Box sx={boxStyle}>
            <Container sx={{display: "flex", flexDirection: "column", alignItems:"center", height:"120px"}}>
                <Typography sx={{ fontSize: "30px", fontWeight: "600", color: "black" }}>{city.name}</Typography>
                <Typography sx={{ fontSize: "20px", fontWeight: "600", color: "#FBC40E" }}>{city.country}, {city.zipCode}</Typography>
            </Container>
            <Button sx={{ ...buttonStyle, mt: 2 }} onClick={() => onButtonClick(city.id)}>View Consumption</Button>
        </Box>
    );
};

export default CityConsumptionCard;