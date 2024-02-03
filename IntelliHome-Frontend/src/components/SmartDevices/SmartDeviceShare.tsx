import React, {useEffect, useState} from "react";
import axios from "axios";
import {environment} from "../../utils/Environment";
import {Box, Button, Chip, IconButton, TextField, Typography} from "@mui/material";
import {Add} from "@mui/icons-material";
import {useMutation, useQuery, useQueryClient} from "react-query";

const SmartDeviceShare = ({smartDeviceId, onClose}) => {
    const[sharedList,setSharedList]=useState([])
    const[emailField,setEmailField]=useState("")
    const [error, setError] = useState<string | null>(null);
    const queryClient = useQueryClient()

    const _ = useQuery({
        queryKey: ['sharedDeviceList'],
        queryFn: () => {
            return axios.get(environment + `/api/SmartDevice/GetSharedList?deviceid=${smartDeviceId}`).then((res) => {
                setSharedList(res.data)
            })
        },
    })

    const deletePermissionMutation = useMutation({
        mutationFn: (data: any) => {
            return axios.put(environment + '/api/SmartDevice/RemovePermission', data).then(() => {
                queryClient.invalidateQueries({queryKey: ['sharedDeviceList']})

            })
        },
    })

    const addPermissionMutation = useMutation({
        mutationFn: (data: any) => {
            return axios.put(environment + '/api/SmartDevice/AddPermission', data).then(() => {
                queryClient.invalidateQueries({queryKey: ['sharedDeviceList']})
                setEmailField("")
            }).catch(() => setError("User with this mail doesen't exist!"));
        },
    })


    return <Box padding={3} display="flex" flexDirection={"column"} alignItems="center" overflow="auto">
        <Typography sx={{ textAlign: "center", width: 1, marginBottom: 2, fontWeight:"Bold", fontSize:"25px", marginTop:0 }}>
            Smart Device Sharing
        </Typography>
        <Box display="flex" justifyConent="center" flexDirection="column">
            <Box display="flex" justifyContent="center" alignItems="center">
                <TextField onChange={(e) => {
                    setEmailField(e.target.value)}}
                           fullWidth
                           id="Email"
                           label="User email"
                           name="Email"
                           type="email"/>
                <IconButton sx={{marginLeft:"10px"}} onClick={()=>addPermissionMutation.mutate({user:emailField,home:smartDeviceId})}>
                    <Add/></IconButton>
            </Box>
            {error && (
                <Typography variant="body2" color="error" sx={{ textAlign: "left", width: 1, marginY: 1 }}>
                    {error}
                </Typography>
            )}
        </Box>
        <Box width="100%" mt={2}>
            {sharedList && sharedList.map((email)=><Chip key={email} label={email} onDelete={()=>{deletePermissionMutation.mutate({user:email,home:smartDeviceId})}} />)}
        </Box>
        <Button
            variant="contained"
            color="primary"
            sx={{mt:2}}
            onClick={()=> onClose()}
        >
            Close
        </Button>
    </Box>
}

export default SmartDeviceShare