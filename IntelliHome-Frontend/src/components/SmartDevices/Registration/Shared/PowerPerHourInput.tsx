import React, { useState } from 'react';
import {
    FormControl,
    FormControlLabel,
    Radio,
    RadioGroup,
    TextField,
} from '@mui/material';
import InputAdornment from '@mui/material/InputAdornment';

interface PowerVisibilityComponentProps {
    onValueChange: (PowerValue: number) => void;
}

const PowerPerHourInput: React.FC<PowerVisibilityComponentProps> = ({onValueChange}) => {
    const [isPowerVisible, setIsPowerVisible] = useState(true);
    const [powerPerHour, setPowerPerHour] = useState(0);

    const handlePowerVisibilityChange = (
        e: React.ChangeEvent<HTMLInputElement>
    ) => {
        const isVisible = e.target.value === 'visible';
        setIsPowerVisible(isVisible);
        if (!isVisible){
            setPowerPerHour(0);
            onValueChange(0);
        }
    };

    const handlePowerPerHourChange = (
        e: React.ChangeEvent<HTMLInputElement>
    ) => {
        const value = Number(e.target.value);
        setPowerPerHour(value);
        onValueChange(isPowerVisible ? value : 0);
    };

    return (
        <div>
            <FormControl component="fieldset">
                <RadioGroup
                    aria-label="PowerVisibility"
                    name="PowerVisibility"
                    value={isPowerVisible ? 'visible' : 'hidden'}
                    onChange={handlePowerVisibilityChange}
                    style={{ flexDirection: 'row' }}
                >
                    <FormControlLabel
                        value="visible"
                        control={<Radio />}
                        label="Network power"
                    />
                    <FormControlLabel
                        value="hidden"
                        control={<Radio />}
                        label="Self sufficient"
                    />
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
                    value={powerPerHour}
                    onChange={handlePowerPerHourChange}
                    InputProps={{
                        endAdornment: (
                            <InputAdornment position="end">KWh</InputAdornment>
                        ),
                    }}
                    inputProps={{
                        min: 0,
                    }}
                />
            )}
        </div>
    );
};

export default PowerPerHourInput;
