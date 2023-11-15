import {Box, Button, Link, TextField, Typography} from "@mui/material";
import {CheckCircle, Close} from "@mui/icons-material";
import {useNavigate} from "react-router-dom";
import {useMutation} from "react-query";
import axios from "axios";
import {environment} from "../../security/Environment";
import {useState} from "react";


const AdminRegistration = () => {
    const navigate = useNavigate()
    const [fileData,setFileData] = useState(null);
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
        margin:"15px auto",width:"400px",backgroundColor:"white",borderRadius:"10px"

    }
    const registrationMutation = useMutation({
        mutationFn: (data:FormData) => {
            return axios.post(environment+'/api/User/register', data).then((res)=>{

            })
        },
    })

    const handleChangeFile= (e)=>{
        setFileData(e.target.files[0]);
    }
    const handleSignUp = async (event) => {
        event.preventDefault();
        const formData= new FormData();
        formData.append("firstName", event.target.firstName.value);
        formData.append("lastName", event.target.lastName.value);
        formData.append("email", event.target.email.value);
        formData.append("username", event.target.username.value);
        formData.append("password",event.target.password.value);
        formData.append("image", fileData);

        // Call the mutation function with the form data
        registrationMutation.mutate(formData);
    };
    return<><Box component="form" onSubmit={handleSignUp}  sx={{display:"flex",width:"100%",justifyContent:"center",flexDirection:"column"}}>
        <Button startIcon={fileData===null?<Close style={{color:"red",fontSize:"26px"}}/>:<CheckCircle style={{color:"#039F13",fontSize:"26px"}}/>} sx={{backgroundColor:"transparent", textTransform:"none", width:"400px",fontSize:"26px",fontWeight:"600",paddingY:"10px",margin:"15px auto",borderRadius:"15px", ':hover':{backgroundColor:"transparent"}}}>Upload profile picture
            <input type="file" onChange={handleChangeFile} style={{display: "block",
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
        <TextField name="firstName"   placeholder="First Name" sx={styled}></TextField>
        <TextField name="lastName" placeholder="Last Name" sx={styled}></TextField>
        <TextField name="email" placeholder="Email" sx={styled} mb={4}></TextField>
        <Box height="35px"/>
        <TextField name="username" placeholder="Username" sx={styled}></TextField>
        <TextField name="password" type="password" variant="outlined"  placeholder="Password" sx={styled}></TextField>
        <TextField name="confirmPassword" type="password" variant="outlined"  placeholder="Confirm Password" sx={styled}></TextField>

        <Button type="submit" sx={{backgroundColor:"#FBC40E", width:"400px",fontSize:"22px",fontWeight:"600",paddingY:"10px",margin:"15px auto",borderRadius:"15px", ':hover':{backgroundColor:"#EDB90D"}}}>Sign In</Button>
        </Box></>
}

export default AdminRegistration;