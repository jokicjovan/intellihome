import React, {useEffect, useState} from "react";
import dayjs from "dayjs";
import SignalRSmartHomeService from "../../services/smartDevices/SignalRSmartHomeService";
import axios from "axios";
import {environment} from "../../utils/Environment";
import {Box, Chip, IconButton, TextField} from "@mui/material";
import {Chart} from "react-google-charts";
import SmartDeviceReportValues from "../SmartDevices/Control/Shared/SmartDeviceReportValues";
import {Add} from "@mui/icons-material";
import {useMutation, useQuery, useQueryClient} from "react-query";

const SmartHomeShare = ({smartHomeId}) => {
    const[sharedList,setSharedList]=useState([])
    const[emailField,setEmailField]=useState("")
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

            })
        },
    })


    return <Box mt={1} padding={3} display="flex" flexDirection={"column"} alignItems="center" overflow="auto">

        <Box display="flex" justifyContent="center" alignItems="center"><TextField onChange={(e) => {
            setEmailField(e.target.value)}} fullWidth/><IconButton sx={{marginLeft:"10px"}} onClick={()=>addPermissionMutation.mutate({user:emailField,home:smartHomeId})}><Add/></IconButton></Box>
        <Box width="100%" mt={2}>
            {sharedList.map((email)=><Chip key={email} label={email} onDelete={()=>{deletePermissionMutation.mutate({user:email,home:smartHomeId})}} />)}

        </Box>
        </Box>
}

export default SmartHomeShare