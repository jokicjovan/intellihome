import {Box, Button, IconButton, TextField, Typography} from "@mui/material";
import {CheckCircle, Close, Shuffle} from "@mui/icons-material";
import {useMutation, useQueryClient} from "react-query";
import axios from "axios";
import {environment} from "../../security/Environment";
import {useState} from "react";
import InputAdornment from "@mui/material/InputAdornment";

const AdminRegistration = () => {
    const queryClient = useQueryClient()
    const initialState = {
        firstName: "",
        lastName: "",
        email: "",
        username: "",
        password: "",
        confirmPassword: "",
    };

    const [formData, setFormData] = useState(initialState);
    const [errorName, setErrorName] = useState(false);
    const [errorLastName, setErrorLastname] = useState(false);
    const [errorMail, setErrorMail] = useState(false);
    const [errorUsername, setErrorUsername] = useState(false);
    const [errorPassword, setErrorPassword] = useState(false);
    const [errorSamePassword, setErrorSamePassword] = useState(false);
    const [axiosError, setAxiosError] = useState("");
    const [errorFile, setErrorFile] = useState(false);
    const [fileData, setFileData] = useState(null);

    const styled = {
        "& label.Mui-focused": {
            color: "#FBC40E"
        },
        "& .MuiOutlinedInput-root": {
            "&.Mui-focused fieldset": {
                borderColor: "#FBC40E",
                borderRadius: "10px"

            },
            borderRadius: "10px"
        },
        margin: "8px auto", width: "400px", borderRadius: "10px"

    }
    const registrationMutation = useMutation({
        mutationFn: (data: FormData) => {
            return axios.post(environment + '/api/User/addAdmin', data).then(() => {
                queryClient.invalidateQueries({queryKey: ['adminList']})
                setFormData(initialState)
                setFileData(null)
                setAxiosError('');

            }).catch((e) => {
                if (e.response.status == 400) {
                    setAxiosError(e.response.data.message);
                }
            })
        },
    })


    const handleChangeFile = (e) => {
        setFileData(e.target.files[0]);
    }
    const handleSignUp = async (event) => {
        event.preventDefault();
        const mailRegex = new RegExp(/^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/);
        const passwordRegex = new RegExp(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$/);
        let hasError = false;
        if (event.target.firstName.value.trim() == "") {
            setErrorName(true)
            hasError = true;
        } else {
            setErrorName(false)
        }
        if (event.target.lastName.value.trim() == "") {
            setErrorLastname(true)
            hasError = true;
        } else {
            setErrorLastname(false)
        }
        if (event.target.email.value.trim() == "" || !mailRegex.test(event.target.email.value)) {
            setErrorMail(true)
            hasError = true;
        } else {
            setErrorMail(false)
        }
        if (event.target.username.value.trim() == "" || event.target.username.value.trim() < 5) {
            setErrorUsername(true)
            hasError = true;
        } else {
            setErrorUsername(false)
        }
        if (event.target.password.value.trim() == "" || !passwordRegex.test(event.target.password.value)) {
            setErrorPassword(true)
            hasError = true;
        } else {
            setErrorPassword(false)
        }
        if (event.target.password.value !== event.target.confirmPassword.value) {
            setErrorSamePassword(true)
            hasError = true;
        } else {
            setErrorSamePassword(false)
        }
        if (fileData == null) {
            setErrorFile(true)
            hasError = true;
        } else {
            setErrorFile(false)
        }

        if (!hasError) {
            const formData = new FormData();
            formData.append("firstName", event.target.firstName.value);
            formData.append("lastName", event.target.lastName.value);
            formData.append("email", event.target.email.value);
            formData.append("username", event.target.username.value);
            formData.append("password", event.target.password.value);
            formData.append("image", fileData);
            registrationMutation.mutate(formData, event);
        }
    };
    const generateRandomPassword = (): string => {
        const length: number = 10;
        const uppercaseLetters: string = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
        const lowercaseLetters: string = 'abcdefghijklmnopqrstuvwxyz';
        const numbers: string = '0123456789';

        let password: string = '';

        // Ensure at least one uppercase letter
        password += uppercaseLetters[Math.floor(Math.random() * uppercaseLetters.length)];

        // Ensure at least one lowercase letter
        password += lowercaseLetters[Math.floor(Math.random() * lowercaseLetters.length)];

        // Ensure at least one number
        password += numbers[Math.floor(Math.random() * numbers.length)];

        // Generate the remaining characters
        for (let i: number = password.length; i < length; i++) {
            const allCharacters: string = uppercaseLetters + lowercaseLetters + numbers;
            password += allCharacters[Math.floor(Math.random() * allCharacters.length)];
        }

        // Shuffle the characters to make the password more random
        password = password.split('').sort(() => Math.random() - 0.5).join('');

        return password;
    }
    return<><Box component="form" onSubmit={handleSignUp} sx={{ display: "flex", width: "100%", justifyContent: "center", flexDirection: "column" }}>
        <TextField name="firstName" value={formData.firstName} onChange={(e) => setFormData({ ...formData, firstName: e.target.value })} placeholder="First Name" error={errorName} helperText={errorName ? "Name is required" : ""} sx={styled}></TextField>
        <TextField name="lastName" value={formData.lastName} onChange={(e) => setFormData({ ...formData, lastName: e.target.value })} placeholder="Last Name" error={errorLastName} helperText={errorLastName ? "Lastname is required" : ""} sx={styled}></TextField>
        <TextField type="email" name="email" value={formData.email} onChange={(e) => setFormData({ ...formData, email: e.target.value })} placeholder="Email" sx={styled} error={errorMail} helperText={errorMail ? "Email is required and must be in the format example@example.com" : ""}></TextField>
        <Box height="25px" />
        <TextField name="username" value={formData.username} onChange={(e) => setFormData({ ...formData, username: e.target.value })} placeholder="Username" error={errorUsername} helperText={errorUsername ? "Username must be at least 5 characters long" : ""} sx={styled}></TextField>
        <TextField name="password" InputProps={{
            endAdornment: (
                <InputAdornment position="end">
                    <IconButton onClick={()=>{
                        const pass=generateRandomPassword()
                        setFormData({...formData,password:pass,confirmPassword: pass})}}>
                        <Shuffle  />
                    </IconButton>
                </InputAdornment>
            ),
        }} type="password" value={formData.password} onChange={(e) => setFormData({ ...formData, password: e.target.value })} variant="outlined" placeholder="Password" error={errorPassword} helperText={errorPassword ? "Password must have 8 characters (1 number, 1 uppercase, and 1 lowercase)" : ""} sx={styled}></TextField>
        <TextField name="confirmPassword" type="password" value={formData.confirmPassword} onChange={(e) => setFormData({ ...formData, confirmPassword: e.target.value })} variant="outlined" placeholder="Confirm Password" error={errorSamePassword} helperText={errorSamePassword ? "Passwords do not match" : ""} sx={styled}></TextField>
        <Button startIcon={fileData === null ? <Close style={{ color: "red", fontSize: "26px" }} /> : <CheckCircle style={{ color: "#039F13", fontSize: "26px" }} />} sx={{ backgroundColor: "transparent",color:"black", textTransform: "none", width: "400px", fontSize: "26px", fontWeight: "600", paddingY: "10px", margin: "15px auto", borderRadius: "15px", ':hover': { backgroundColor: "transparent" } }}>Upload profile picture
            <input type="file" onChange={handleChangeFile} style={{ display: "block", height: "100%", width: "100%", position: "absolute", top: 0, bottom: 0, left: 0, right: 0, opacity: 0, cursor: "pointer" }} />
        </Button>
        {errorFile && <Typography align="center" sx={{ fontSize: "0.75rem", fontWeight: "400", color: "#d32f2f" }}>File not uploaded</Typography>}
        {axiosError === 'User with that email already exists!' && <Typography align="center" sx={{ fontSize: "0.75rem", fontWeight: "400", color: "#d32f2f" }}>Email already in use</Typography>}
        {axiosError === 'User with that username already exists!' && <Typography align="center" sx={{ fontSize: "0.75rem", fontWeight: "400", color: "#d32f2f" }}>Username already in use</Typography>}
        {axiosError !== '' && axiosError !== 'User with that email already exists!' && axiosError !== 'User with that username already exists!' && <Typography align="center" sx={{ fontSize: "0.75rem", fontWeight: "400", color: "#d32f2f" }}>Something went wrong</Typography>}

        <Button type="submit" sx={{ backgroundColor: "#FBC40E",color:"black", width: "400px", fontSize: "22px", fontWeight: "600", paddingY: "10px", margin: "15px auto", borderRadius: "15px", ':hover': { backgroundColor: "#EDB90D" } }}>Sign Up Admin</Button>
    </Box></>
}

export default AdminRegistration;