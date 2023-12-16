import {Box, Button, Container, Dialog, Typography} from '@mui/material';
import {Apartment, Check, Clear, Devices, LocationOn, Person, PhotoSizeSelectSmall} from "@mui/icons-material";
import { environment } from "../../security/Environment.tsx";
import React from "react";
import axios from "axios";

const SmartHomeApprovalCard = (props) => {
    const data = props.data
    const onChangeHendler = props.onChangeHendler
    const textStyle = { fontStyle: "bold", fontWeight: "600", color: "black", margin: "5px" };
    const iconStyle = { fontStyle: "bold", fontWeight: "600", color: "#343F71", margin: "5px" };
    const containerStyle = { display: "flex", flexDirection: "row", padding: "0 10px", alignItems: "center" };
    const [openModal, setOpenModal] = React.useState(false);

    const buttonStyle = {
        width: "150px",
        backgroundColor: "#DBDDEB",
        color: "#343F71",
        fontWeight: "600",
        textTransform: "none",
        margin: "10px 10px",
        display: "block",
        borderRadius: "5px",
        boxShadow: "0 2px 5px rgba(0, 0, 0, 0.2)",
        padding: 0
    }
    const boxStyle = {
        width: "18vw",
        height: "37vh",
        backgroundColor: "white",
        borderRadius: "15px",
        textAlign: "center",
        mb: "10px",
        display: "flex",
        flexDirection: "column",
    }

    const handleCloseModal = () => {
        setOpenModal(false);
    }

    const handleOpenModal = () => {
        setOpenModal(true);
    }

    const button1Handler = () => {
        handleCloseModal();
    }

    const button2Handler = () => {
        const deleteParams = {
            params: {
                Id: data.id,
                UserId: data.owner.id,
                Reason: (document.getElementById("reason-text") as HTMLInputElement).value
            }
        };

        axios.delete(`${environment}/api/SmartHome/DeleteSmartHome`, deleteParams)
            .then(res => {
                if (res.status === 200) {
                    onChangeHendler(data.id);
                }
            })
            .catch(error => {
                console.log(error);
            });

        handleCloseModal();
    };


    const handleApproval = () => {
        axios.put(`${environment}/api/SmartHome/ApproveSmartHome`, null, {
            params: {
                Id: data.id,
            },
        }).then(res => {
            if (res.status === 200) {
                onChangeHendler(data.id);
            }
        }).catch((error) => {
            console.log(error);
        });
    }


    const modalContent = (
        <Box sx={{ width: "50vw", height: "40vh", backgroundColor: "white", borderRadius: "10px", padding: "20px", display: "flex", flexDirection: "column", position: "relative"}}>
            <Typography sx={{ fontSize: "30px", fontWeight: "600", margin: "10px" }}>Reason for declined property</Typography>
            {/* Content */}

            <textarea id="reason-text" style={{width: "95%", height: "100%", border: "1px solid #343F71", borderRadius: "5px", padding: "10px", resize: "none", margin:"auto"}}></textarea>

            {/* Buttons */}
            <Box sx={{ display: "flex", justifyContent: "flex-end", alignItems: "center", mt:"30px" }}>
                <Button onClick={button1Handler} sx={{ marginRight: "20px", background: "white", width: "7vw", height: "5vh", color: "black", border: '1px solid #FBC40E', fontWeight: "500", textTransform: "none", fontSize: "1.4rem" }}>Cancel</Button>
                <Button onClick={button2Handler} sx={{ background: "#FBC40E", width: "7vw", height: "5vh", fontWeight: "500", textTransform: "none", fontSize: "1.4rem" }}>Submit</Button>
            </Box>
        </Box>
    );

    return (
        <Box sx={boxStyle}>
            <Container disableGutters sx={{ display: "flex", flexDirection: "row", marginY: "0vh", marginX: "5px 0" }}>
                <img
                    src={environment + '/' + data.image}
                    alt="Smart Home Image"
                    style={{ width: "50px", height: "50px", border: "5px solid #343F71", borderRadius: "8px", margin: "10px" }}
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
            <Container disableGutters sx={containerStyle}>
                <Person sx={iconStyle} /><Typography sx={textStyle}>{data.owner.firstName} {data.owner.lastName}</Typography>
            </Container>
            <Container disableGutters sx={{...containerStyle, mt: 'auto'}}>
                <Button onClick={handleApproval}sx={{ ...buttonStyle}}><Check sx={{fontStyle: "bold", fontWeight: "600", color: "green", margin: "5px"}}/></Button>
                <Button onClick={handleOpenModal} sx={{ ...buttonStyle}}><Clear sx={{fontStyle: "bold", fontWeight: "600", color: "red", margin: "5px" }}/></Button>
            </Container>

            <Dialog  maxWidth="xl" open={openModal} onClose={handleCloseModal}>
                {modalContent}
            </Dialog>
        </Box>


    );
};

export default SmartHomeApprovalCard;
