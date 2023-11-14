import React, {useState} from 'react';
import {Button, FormControl, FormControlLabel, Radio, RadioGroup, TextField} from "@mui/material";
import InputAdornment from "@mui/material/InputAdornment";
import CommonSmartDeviceFields from "./CommonSmartDeviceFields.ts";
import {CheckCircle, Close} from "@mui/icons-material";

interface SmartDeviceRegistrationFormProps {
    formData: CommonSmartDeviceFields;
    onFormChange: (data: CommonSmartDeviceFields) => void;
}

const SmartDeviceRegistrationForm: React.FC<SmartDeviceRegistrationFormProps> = ({ formData, onFormChange }) => {
    const [isPowerVisible, setIsPowerVisible] = useState(true);
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

    const handleVisibilityChange = (e: any) => {
        setIsPowerVisible(e.target.value === 'visible');
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

            <FormControl component="fieldset">
                <RadioGroup
                    aria-label="PowerVisibility"
                    name="PowerVisibility"
                    value={isPowerVisible ? 'visible' : 'hidden'}
                    onChange={handleVisibilityChange}
                    style={{ flexDirection: 'row' }}
                >
                    <FormControlLabel value="visible" control={<Radio />} label="Network power" />
                    <FormControlLabel value="hidden" control={<Radio />} label="Self sufficient" />
                </RadioGroup>
            </FormControl>

            {isPowerVisible && (
                <TextField
                    variant="outlined"
                    margin="normal"
                    required
                    fullWidth
                    id="PowerPerHour"
                    label="Power per hour"
                    name="PowerPerHour"
                    type="number"
                    value={formData.PowerPerHour}
                    onChange={handleInputChange}
                    InputProps={{
                        endAdornment: <InputAdornment position="end">KWh</InputAdornment>,
                    }}
                    inputProps={{
                        min: 0,
                    }}
                />
            )}
            <Button startIcon={formData.Image.size === 0 ? <Close style={{color:"red",fontSize:"26px"}}/>:
                <CheckCircle style={{color:"#039F13",fontSize:"26px"}}/>}
                    sx={{backgroundColor:"transparent",
                        textTransform:"none",
                        width:"400px",
                        fontSize:"26px",
                        fontWeight:"600",
                        paddingY:"10px",
                        margin:"15px auto",
                        borderRadius:"15px",
                        '&:hover': {
                            backgroundColor: 'gray',
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