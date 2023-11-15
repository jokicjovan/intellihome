import React from 'react';
import { Box, Button } from '@mui/material';

interface DeviceRegistrationButtonsProps {
    onCancel: () => void;
}

const DeviceRegistrationButtons: React.FC<DeviceRegistrationButtonsProps> = ({ onCancel }) => {
    return (
        <Box
            sx={{
                display: 'flex',
                justifyContent: 'end',
                margin: 1,
                marginBottom: 0,
                width: 1,
            }}
        >
            <Button
                variant="contained"
                color="primary"
                sx={{
                    marginRight: 2,
                    backgroundColor: 'white',
                    border: 1,
                    '&:hover': {
                        backgroundColor: 'lightGray',
                    },
                }}
                onClick={onCancel}
            >
                Cancel
            </Button>
            <Button
                type="submit"
                variant="contained"
                color="primary"
                sx={{
                    border: 1,
                }}
            >
                Create
            </Button>
        </Box>
    );
};

export default DeviceRegistrationButtons;
