import React, {useEffect, useState} from "react";
import axios from "axios";
import {environment} from "../../utils/Environment";
import {Box, Chip, IconButton, TextField} from "@mui/material";
import {Add} from "@mui/icons-material";
import {useMutation, useQuery, useQueryClient} from "react-query";

const SmartDeviceShare = ({smartDeviceId}) => {
    const[sharedList,setSharedList]=useState([])
    const[emailField,setEmailField]=useState("")
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

            })
        },
    })


    return <Box mt={1} padding={3} display="flex" flexDirection={"column"} alignItems="center" overflow="auto">

        <Box display="flex" justifyContent="center" alignItems="center"><TextField onChange={(e) => {
            setEmailField(e.target.value)}} fullWidth/><IconButton sx={{marginLeft:"10px"}} onClick={()=>addPermissionMutation.mutate({user:emailField,home:smartDeviceId})}><Add/></IconButton></Box>
        <Box width="100%" mt={2}>
            {sharedList && sharedList.map((email)=><Chip key={email} label={email} onDelete={()=>{deletePermissionMutation.mutate({user:email,home:smartDeviceId})}} />)}

        </Box>
    </Box>
}

export default SmartDeviceShare