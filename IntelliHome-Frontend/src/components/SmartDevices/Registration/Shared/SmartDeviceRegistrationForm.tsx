import React from 'react';
import {Button, TextField, useTheme} from "@mui/material";
import CommonSmartDeviceFields from "../../../../models/interfaces/CommonSmartDeviceFields.ts";
import {CheckCircle, Close} from "@mui/icons-material";

interface SmartDeviceRegistrationFormProps {
    formData: CommonSmartDeviceFields;
    onFormChange: (data: CommonSmartDeviceFields) => void;
}

const SmartDeviceRegistrationForm: React.FC<SmartDeviceRegistrationFormProps> = ({ formData, onFormChange }) => {
    const theme = useTheme();
    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target;
        onFormChange({ ...formData, [name]: value });
    };

    const handleImageUpload = async (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0];
        if (file) {
            onFormChange({ ...formData, Image: file });
        }
    };

    return (
        <div>
            <TextField
                variant="outlined"
                margin="normal"
                required
                fullWidth
                id="Name"
                label="Name"
                name="Name"
                type="text"
                value={formData.Name}
                onChange={handleInputChange}
            />

            <Button startIcon={formData.Image.size === 0 ?
                <Close style={{color:"red",fontSize:"26px"}}/>:
                <CheckCircle style={{color:"#039F13",fontSize:"26px"}}/>}
                    sx={{backgroundColor:"transparent",
                        textTransform:"none",
                        width:"400px",
                        fontSize:"26px",
                        fontWeight:"600",
                        paddingY:"10px",
                        margin:"15px auto",
                        borderRadius:"15px",
                        border:1,
                        borderColor:"lightGray",
                        '&:hover': {
                            backgroundColor: theme.palette.secondary.main,
                        },}}
            >Upload device picture
                <input type="file" onChange={handleImageUpload} style={{display: "block",
                    height: "100%",
                    width: "100%",
                    position: "absolute",
                    top: 0,
                    bottom: 0,
                    left: 0,
                    right: 0,
                    opacity: 0,
                    cursor: "pointer"}}/>
            </Button>
        </div>
    );
};

export default SmartDeviceRegistrationForm;