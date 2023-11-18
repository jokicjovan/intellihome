import {Box, Button, TextField, Typography} from "@mui/material";
import React, {useContext, useState} from "react";
import axios from "axios";
import {environment} from "../../security/Environment";
import {useMutation, useQueryClient} from "react-query";
import {useNavigate} from "react-router-dom";
import {AuthContext} from "../../security/AuthContext";


const AdminChangePassword = () => {
    const {id}=useContext(AuthContext)
    const queryClient = useQueryClient()
    const navigate = useNavigate()
    const initialState = {
        password: "",
        confirmPassword: "",
    };

    const [formData, setFormData] = useState(initialState);
    const [errorPassword,setErrorPassword]=useState(false);
    const [errorSamePassword,setErrorSamePassword]=useState(false);
    const [axiosError,setAxiosError]=useState("");

    const styled = {
        "& label.Mui-focused": {
            color: "#FBC40E"
        },
        "& .MuiOutlinedInput-root": {
            "&.Mui-focused fieldset": {
                borderColor: "#FBC40E",
                borderRadius:"10px"

            },
            borderRadius:"10px"
        },
        margin:"8px auto",width:"400px",borderRadius:"10px"

    }
    const registrationMutation = useMutation({
        mutationFn: (data:any) => {
            return axios.post(environment+'/api/User/changePassword', data).then((res)=>{
                setFormData(initialState)
                setAxiosError('');
                if (res.status==200) navigate(0)

            }).catch((e)=>{
                if (e.response.status ==400) {setAxiosError(e.response.data.message);}
            })
        },
    })


    const handleSignUp = async (event) => {
        event.preventDefault();
        const passwordRegex=new RegExp(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$/);
        let hasError=false;

        if(event.target.password.value.trim()=="" ||  !passwordRegex.test(event.target.password.value)){
            setErrorPassword(true)
            hasError=true;
        }else {setErrorPassword(false)}
        if(event.target.password.value!== event.target.confirmPassword.value){
            setErrorSamePassword(true)
            hasError=true;
        }else {setErrorSamePassword(false)}

        if (!hasError) {
            registrationMutation.mutate({id:id,password:event.target.password.value},event);
        }
    };
    return<><Box component="form" onSubmit={handleSignUp} sx={{ display: "flex", width: "100%", justifyContent: "center", flexDirection: "column" }}>
        <TextField name="password" type="password" value={formData.password} onChange={(e) => setFormData({ ...formData, password: e.target.value })} variant="outlined" placeholder="Password" error={errorPassword} helperText={errorPassword ? "Password must have 8 characters (1 number, 1 uppercase, and 1 lowercase)" : ""} sx={styled}></TextField>
        <TextField name="confirmPassword" type="password" value={formData.confirmPassword} onChange={(e) => setFormData({ ...formData, confirmPassword: e.target.value })} variant="outlined" placeholder="Confirm Password" error={errorSamePassword} helperText={errorSamePassword ? "Passwords do not match" : ""} sx={styled}></TextField>
        {axiosError !== '' && <Typography align="center" sx={{ fontSize: "0.75rem", fontWeight: "400", color: "#d32f2f" }}>Something went wrong</Typography>}

        <Button type="submit" sx={{ backgroundColor: "#FBC40E",color:"black", width: "400px", fontSize: "22px", fontWeight: "600", paddingY: "10px", margin: "15px auto", borderRadius: "15px", ':hover': { backgroundColor: "#EDB90D" } }}>Change Password</Button>
    </Box></>
}

export default AdminChangePassword;