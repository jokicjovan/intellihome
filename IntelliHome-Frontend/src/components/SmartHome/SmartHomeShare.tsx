import React, {useEffect, useState} from "react";
import dayjs from "dayjs";
import SignalRSmartHomeService from "../../services/smartDevices/SignalRSmartHomeService";
import axios from "axios";
import {environment} from "../../utils/Environment";
import {Box, Button, Chip, IconButton, TextField, Typography} from "@mui/material";
import {Add} from "@mui/icons-material";
import {useMutation, useQuery, useQueryClient} from "react-query";

const SmartHomeShare = ({smartHomeId, onClose}) => {
    const[sharedList,setSharedList]=useState([])
    const[emailField,setEmailField]=useState("")
    const [error, setError] = useState<string | null>(null);
    const queryClient = useQueryClient()

    const _ = useQuery({
        queryKey: ['sharedHomeList'],
        queryFn: () => {
            return axios.get(environment + `/api/SmartHome/GetAllEmailsWithPermission?homeId=${smartHomeId}`).then((res) => {
                setSharedList(res.data)
            })
        },
    })

    const deletePermissionMutation = useMutation({
        mutationFn: (data: any) => {
            return axios.put(environment + '/api/SmartHome/RemovePermission', data).then(() => {
                queryClient.invalidateQueries({queryKey: ['sharedHomeList']})

            })
        },
    })

    const addPermissionMutation = useMutation({
        mutationFn: (data: any) => {
            return axios.put(environment + '/api/SmartHome/AddPermission', data).then(() => {
                queryClient.invalidateQueries({queryKey: ['sharedHomeList']})
                setEmailField("")

            }).catch(() => setError("User with this mail doesen't exist!"));
        },
    })


    return <Box padding={3} display="flex" flexDirection={"column"} alignItems="center" overflow="auto">
        <Typography sx={{ textAlign: "center", width: 1, marginBottom: 2, fontWeight:"Bold", fontSize:"25px", marginTop:0 }}>
            Smart Home Sharing
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
                <IconButton sx={{marginLeft:"10px"}} onClick={()=>addPermissionMutation.mutate({user:emailField,home:smartHomeId})}><Add/></IconButton>
            </Box>
            {error && (
                <Typography variant="body2" color="error" sx={{ textAlign: "left", width: 1, marginY: 1 }}>
                    {error}
                </Typography>
            )}
        </Box>
            <Box width="100%" mt={2}>
                {sharedList.map((email)=><Chip key={email} label={email} onDelete={()=>{deletePermissionMutation.mutate({user:email,home:smartHomeId})}} />)}

            </Box>
        <Button
            variant="contained"
            color="primary"
            onClick={()=> onClose()}
            sx={{mt:2}}
        >
            Close
        </Button>
        </Box>

}

export default SmartHomeShare